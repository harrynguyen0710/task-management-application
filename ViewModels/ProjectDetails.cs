using task_management.Models;

namespace task_management.ViewModels
{
    public class ProjectDetails
    {
        public Project Project { get; set; }
        public List<UserRoles> UserRole { get; set; }
        public IEnumerable<Tasks> Tasks { get; set; }
        public Tasks SelectedTask { get; set; }

        // Pagination properties
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalTasks { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalTasks / PageSize);
    }
}
