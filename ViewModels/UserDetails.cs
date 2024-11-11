using task_management.Models;

namespace task_management.ViewModels
{
    public class UserDetails
    {
        public Users User { get; set; }
        public IEnumerable<Tasks> Tasks {  get; set; } 
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<string> RoleNames { get; set; }  
    }
}
