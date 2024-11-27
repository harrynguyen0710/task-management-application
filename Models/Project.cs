using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management.Models
{
    public class Project
    {
        [Key]
        public int projectId { get; set; }
        [Required(ErrorMessage = "Please enter project name")]
        [Display(Name = "Project name")]
        public string name { get; set; }
        [Required(ErrorMessage = "Please enter project description")]
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "Start date")]
        public DateTime startDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Please select project status")]
        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Progress")]
        public int? progress { get; set; } = 0;

        [Display(Name = "Target")]
        [Required(ErrorMessage = "Please enter project target")]
        public string target { get; set; }

        [ForeignKey("Organization")]
        public int organizationId { get; set; }
        public bool isActive { get; set; }
        public Organization Organization { get; set; }
        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }
    }
}
