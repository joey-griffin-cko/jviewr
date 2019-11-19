namespace api.Services
{
    public interface IDataService
    {
        void SetGithubToken(string token);
        void SetJiraToken(string token);
        string GetGithubToken();
        string GetJiraToken();
        void SetGithubTokenValidity(bool isValid);
        void SetJiraTokenValidity(bool isValid);

        bool GithubTokenIsValid();
        bool JiraTokenIsValid();
    }
}