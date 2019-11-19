using System.Collections.Generic;

namespace api.Models
{
    public class JiraResponse
    {        
        public IEnumerable<SprintTask> Issues { get; set; }
    }
}