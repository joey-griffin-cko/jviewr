namespace api.Services
{
    public sealed class DataService : IDataService
    {
        private string _githubToken;
        private string _jiraToken;
        private bool _githubTokenIsValid;
        private bool _jiraTokenIsValid;

        public string GetGithubToken() => _githubToken;
        public string GetJiraToken() => _jiraToken;
        public void SetGithubToken(string token) => _githubToken = token;
        public void SetGithubTokenValidity(bool isValid) => _githubTokenIsValid = isValid;
        public void SetJiraToken(string token) => _jiraToken = token;
        public void SetJiraTokenValidity(bool isValid) => _jiraTokenIsValid = isValid;
        bool IDataService.GithubTokenIsValid() => _githubTokenIsValid;
        bool IDataService.JiraTokenIsValid() => _jiraTokenIsValid;
    }
}