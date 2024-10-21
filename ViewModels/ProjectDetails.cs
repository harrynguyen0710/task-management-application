using Microsoft.AspNet.Identity.EntityFramework;
using task_management.Models;

namespace task_management.ViewModels
{
    public class ProjectDetails
    {
        public Project Project { get; set; }
        public IEnumerable<UserRoles> UserRole { get; set; }
        public IEnumerable<Tasks> Tasks { get; set; }
    }
}
