using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management.Models
{
    public class Tasks
    {
        [Key]
        public int taskId { get; set; }
        [Required(ErrorMessage = "Please enter task name")]
        [Display(Name = "Task name")]
        public string name { get; set; }

        [Required(ErrorMessage = "Please enter task description")]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Start date")]
        public DateTime startDate { get; set; } = DateTime.Now;

        [Display(Name = "Due date")]
        public DateTime dueDate { get; set;} = DateTime.Now;

        [Required(ErrorMessage = "Please select priority mode")]
        [Display(Name = "Priority")]
        public string priority { get; set; }
        
        [Required(ErrorMessage = "Please select status mode")]
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Progress")]
        public int? progress { get; set; } = 0;

        [Display(Name = "Feedback")]
        public string? feedback { get; set; }
        public bool isActive { get; set; } = true;

        [ForeignKey("ProjectAssignments")]
        public int projectId { get; set; }
        public string userId { get; set; }


        public virtual ProjectAssignment ProjectAssignment { get; set; }    


       
    }
}
