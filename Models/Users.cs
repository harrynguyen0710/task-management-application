using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management.Models
{
    public class Users : IdentityUser
    {
        [ForeignKey("Organization")]
        public int organizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }

    }
}
