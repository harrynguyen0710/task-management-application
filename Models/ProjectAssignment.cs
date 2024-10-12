using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management.Models
{
    public class ProjectAssignment
    {
/*        [Key]
        public int id { get; set; }*/
        [ForeignKey("Project")]
        public int projectId { get; set; }
        [ForeignKey("User")]
        public string userId { get; set; }
        public virtual Project Project { get; set; }
        public virtual Users User { get; set; }
    }
}
