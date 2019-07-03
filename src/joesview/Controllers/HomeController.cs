using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using joesview.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Concurrent;

namespace joesview.Controllers
{
    public class HomeController : Controller
    {
        private readonly GithubSettings _githubSettings;
        public HomeController(GithubSettings githubSettings)
        {
            _githubSettings = githubSettings ?? throw new ArgumentNullException(nameof(githubSettings));
        }
        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel
            {
                PRs = await GetPRsAsync(),
                Issues = await GetSprintTaskAsync()
            };

            return View(vm);
        }

        private async Task<IEnumerable<IGrouping<string,GitThing>>> GetPRsAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"token {_githubSettings.Token}");
            client.DefaultRequestHeaders.Add("Accept", $"application/vnd.github.sailor-v-preview+json");
            client.DefaultRequestHeaders.Add("User-Agent", $"PostmanRuntime/7.13.0");
            client.DefaultRequestHeaders.Add("Cache-Control", $"no-cache");
            client.DefaultRequestHeaders.Add("Postman-Token", $"f616a897-2e99-46d2-8ef9-6f6dab85e22f");
            client.DefaultRequestHeaders.Add("Host", $"api.github.com");
            client.DefaultRequestHeaders.Add("Connection", $"keep-alive");

            var allPrs = new ConcurrentBag<GitThing>();
            var totalPrs = 0;

            var repos = _githubSettings.Repositories;

            async Task<(HttpResponseMessage Response, string Repo)> GetRepos(string repo)
            {
                return (await client.GetAsync($"https://api.github.com/repos/{repo}/pulls?state=open"), repo);
            }

            async Task AddPRs(HttpResponseMessage response, string repo)
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var prs = JsonConvert.DeserializeObject<List<GitThing>>(json);

                    totalPrs += prs.Count;

                    Parallel.ForEach(prs, async (pr) =>
                    {
                        try
                        {
                            pr.Repo = repo;

                            var reviewResponse = await client.GetAsync($"https://api.github.com/repos/{repo}/pulls/{pr.Number}/reviews");
                            var reviewJson = await reviewResponse.Content.ReadAsStringAsync();
                            var reviews = JsonConvert.DeserializeObject<List<Review>>(reviewJson);

                            pr.Reviews = reviews;
                            allPrs.Add(pr);
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            totalPrs--;
                        }
                    });
                }
            }

            var repoTasks = repos.Select(repo => GetRepos(repo));

            var prTasks = repoTasks.Select(async task =>
            {
                var result = await task;
                await AddPRs(result.Response, result.Repo);
            });

            await Task.WhenAll(prTasks);

            while (totalPrs > 0)
            {

            }

            return allPrs.GroupBy(x => x.Repo);
        }

        private async Task<IEnumerable<IGrouping<string,SprintTask>>> GetSprintTaskAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Basic am9leS5ncmlmZmluQGNoZWNrb3V0LmNvbTpBQUpKYWhGeWFZalNXMzFBM1lTV0I1RkE=");
            client.DefaultRequestHeaders.Add("Accept", $"application/json");
            client.DefaultRequestHeaders.Add("User-Agent", $"PostmanRuntime/7.13.0");
            client.DefaultRequestHeaders.Add("Cache-Control", $"no-cache");
            client.DefaultRequestHeaders.Add("Postman-Token", $"f616a897-2e99-46d2-8ef9-6f6dab85e22f");
            client.DefaultRequestHeaders.Add("Host", $"checkout.atlassian.net");
            client.DefaultRequestHeaders.Add("Connection", $"keep-alive");

            var boardResponse = await client.GetAsync("https://checkout.atlassian.net/rest/agile/1.0/board/112/sprint?state=active");

            var boardId = (JsonConvert.DeserializeObject<dynamic>(await boardResponse.Content.ReadAsStringAsync())).values[0].id;

            var response = await client.GetAsync($"https://checkout.atlassian.net/rest/agile/1.0/sprint/{boardId}/issue?fields=status,summary");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<JiraResponse>(content);

                return res.Issues.OrderBy(x => x.Fields.Status.Name).GroupBy(x => x.Fields.Status.Name);
            }

            return null;
        }
    }
}