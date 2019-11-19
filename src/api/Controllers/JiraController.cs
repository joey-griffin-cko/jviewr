using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraController : Controller
    {
        private readonly IDataService _dataService;
        public JiraController(IDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpPost("{credential:minlength(10)}")]
        public IActionResult SetToken(string credential)
        {
            var bytes = Encoding.UTF8.GetBytes(credential);
            var token = Convert.ToBase64String(bytes);

            _dataService.SetJiraToken(token);

            return Ok();
        }

        [HttpGet("status")]
        public IActionResult HasValidToken()
        {
            var hasValidToken = _dataService.JiraTokenIsValid();

            if (hasValidToken)
                return Ok();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var tickets = await GetSprintTaskAsync();
            _dataService.SetJiraTokenValidity(tickets != null);

            if (tickets == null)
            {
                return StatusCode(500);
            }

            return Ok(tickets);
        }

        private async Task<IEnumerable<IGrouping<string, SprintTask>>> GetSprintTaskAsync()
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add("Authorization", $"Basic {_dataService.GetJiraToken()}");
                client.DefaultRequestHeaders.Add("Accept", $"application/json");
                client.DefaultRequestHeaders.Add("User-Agent", $"PostmanRuntime/7.13.0");
                client.DefaultRequestHeaders.Add("Cache-Control", $"no-cache");
                client.DefaultRequestHeaders.Add("Postman-Token", $"f616a897-2e99-46d2-8ef9-6f6dab85e22f");
                client.DefaultRequestHeaders.Add("Host", $"checkout.atlassian.net");
                client.DefaultRequestHeaders.Add("Connection", $"keep-alive");

                var boardResponse = await client.GetAsync("https://checkout.atlassian.net/rest/agile/1.0/board/112/sprint?state=active");

                var boardId = (JsonConvert.DeserializeObject<dynamic>(await boardResponse.Content.ReadAsStringAsync())).values[0].id;

                var response = await client.GetAsync($"https://checkout.atlassian.net/rest/agile/1.0/sprint/{boardId}/issue?fields=fixVersions,epic,assignee,status,summary,issuetype,description,comment,customfield_10124");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<JiraResponse>(content);

                    return res.Issues.OrderBy(x => x.Fields.Status.Name).GroupBy(x => x.Fields.Status.Name);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
