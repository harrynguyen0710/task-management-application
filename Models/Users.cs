using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management.Models
{
    public class Users : IdentityUser
    {
        [Required(ErrorMessage = "Please enter user full name")]
        [Display(Name = "Full name")]
        public string fullName { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        [Display(Name = "Address")]
        public string address { get; set; }

        [ForeignKey("Organization")]
        public int organizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }

        [Required(ErrorMessage = "Please upload an user avatar")]
        public string PhotoUrl { get; set; }

        [NotMapped]
        public IFormFile ProfilePhoto { get; set; }


    }
}
