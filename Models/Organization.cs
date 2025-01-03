﻿using System.ComponentModel.DataAnnotations;

namespace task_management.Models
{
    public class Organization
    {
        [Key]
        public int organizationId { get; set; }

        [Required(ErrorMessage = "Please enter organization name")]
        [Display(Name = "Organization name")]
        public string name { get; set; }

        [Required(ErrorMessage = "Please enter organization description")]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required(ErrorMessage = "Please enter organization address")]
        [Display(Name = "Address")]
        public string adress {  get; set; }

        [Required(ErrorMessage = "Please enter organization phone number")]
        [Display(Name = "Phone number")]
        public string phoneNumber { get; set; } 
        public ICollection<Users> Users { get; set; }    
        public ICollection<Project> Projects { get; set; }
    }
}
