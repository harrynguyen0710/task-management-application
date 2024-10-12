using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management.Models
{
    public class Tasks
    {
        [Key]
        public int taskId { get; set; }
        [Required(ErrorMessage = "Please enter task name")]
        public string name { get; set; }
        [Required(ErrorMessage = "Please enter task description")]
        public string description { get; set; }
        public DateTime startDate = DateTime.Now;
        public DateTime dueDate = DateTime.Now;
        [Required(ErrorMessage = "Please select priority mode")]
        public string priority { get; set; }
        [ForeignKey("User")]
        public string userId { get; set; }
        public Users User { get ; set; }
       
    }
}
