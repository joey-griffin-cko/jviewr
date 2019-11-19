namespace api.Models
{
    public class SprintTask
    {
        public string Key { get; set; }
        public string Url => $"https://checkout.atlassian.net/browse/{Key}";
        public JiraField Fields { get; set; }
    }
}