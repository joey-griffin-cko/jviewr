using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : Controller
    {
        private readonly IDataService _dataService;
        public GithubController(IDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpPost("{token:minlength(10)}")]
        public IActionResult SetToken(string token)
        {
            _dataService.SetGithubToken(token);

            return Ok();
        }

        [HttpGet("status")]
        public IActionResult HasValidToken()
        {
            var hasValidToken = _dataService.GithubTokenIsValid();

            if (hasValidToken)
                return Ok();

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var prs = await GetPRsAsync();
            
            _dataService.SetGithubTokenValidity(prs != null);

            if (prs == null)
            {
                return StatusCode(500);
            }

            return Ok(prs);
        }

        private async Task<IEnumerable<IGrouping<string, GitThing>>> GetPRsAsync()
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add("Authorization", $"token {_dataService.GetGithubToken()}");
                client.DefaultRequestHeaders.Add("Accept", $"application/vnd.github.sailor-v-preview+json");
                client.DefaultRequestHeaders.Add("User-Agent", $"PostmanRuntime/7.13.0");
                client.DefaultRequestHeaders.Add("Cache-Control", $"no-cache");
                client.DefaultRequestHeaders.Add("Postman-Token", $"f616a897-2e99-46d2-8ef9-6f6dab85e22f");
                client.DefaultRequestHeaders.Add("Host", $"api.github.com");
                client.DefaultRequestHeaders.Add("Connection", $"keep-alive");

                var allPrs = new ConcurrentBag<GitThing>();
                var totalPrs = 0;

                var repos = new List<string>
                {
                    "CKOTech/checkout-3ds2-sdk-js",
                    "CKOTech/checkout-3ds2-range-tree",
                    "CKOTech/checkout-3ds2-adoption-insights",
                    "CKOTech/checkout-3ds2-business-insights",
                    "CKOTech/checkout-3ds2-eventstore",
                    "CKOTech/checkout-3ds2-authentication",
                    "CKOTech/checkout-3ds2-libs",
                    "CKOTech/checkout-3ds2-infrastructure",
                    "CKOTech/checkout-3ds2-docs",
                    "CKOTech/checkout-3ds2-admin",
                    "CKOTech/checkout-3ds2-interceptor",
                    "CKOTech/checkout-3ds2-expirer",
                    "CKOTech/checkout-3ds2-authentication-sdk",
                    "CKOTech/checkout-3ds2-account-range",
                    "CKOTech/checkout-3ds2-card-ranges",
                    "CKOTech/checkout-3ds2-simulator",
                    "CKOTech/checkout-3ds2-couchbase",
                    "CKOTech/checkout-3ds2-certification",
                    "CKOTech/checkout-acquiring-es",
                    "CKOTech/checkout-acquiring-sqs",
                    "CKOTech/checkout-acquiring-s3",
                    "CKOTech/checkout-acquiring-bootstrap",
                    "CKOTech/checkout-nt-encryption",
                    "CKOTech/checkout-nt-simulator",
                    "CKOTech/checkout-nt-docs"
                };

                async Task<(HttpResponseMessage Response, string Repo)> GetRepos(string repo)
                {
                    return (await client.GetAsync($"https://api.github.com/repos/{repo}/pulls?state=open"), repo);
                }

                async Task AddPRs(HttpResponseMessage response, string repo)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var prs = JsonConvert.DeserializeObject<IEnumerable<GitThing>>(json).OrderBy(x => x.Title);

                        totalPrs += prs.Count();

                        Parallel.ForEach(prs, async (pr) =>
                        {
                            try
                            {
                                pr.Repo = repo;

                                var reviewResponse = await client.GetAsync($"https://api.github.com/repos/{repo}/pulls/{pr.Number}/reviews");
                                var reviewJson = await reviewResponse.Content.ReadAsStringAsync();
                                var reviews = JsonConvert.DeserializeObject<List<Review>>(reviewJson);

                                pr.Reviews = reviews.Where(x => x.State.ToLower() == "approved");
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

                return allPrs.OrderBy(x => x.Url).GroupBy(x => x.Repo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
