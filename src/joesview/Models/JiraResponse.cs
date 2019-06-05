using System.Collections.Generic;

namespace joesview.Models
{
    public class JiraResponse
    {
        public IEnumerable<SprintTask> Issues { get; set; }
    }
}