using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management.Models
{
    public class Project
    {
        [Key]
        public int projectId { get; set; }
        [Required(ErrorMessage = "Please enter project name")]
        public string name { get; set; }
        [Required(ErrorMessage = "Please enter project description")]
        public string description { get; set; }
        public DateTime startDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Please select project status")]
        public string status { get; set; }
        [ForeignKey("Organization")]
        public int organizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }
    }
}
