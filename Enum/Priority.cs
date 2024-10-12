using System.ComponentModel.DataAnnotations;

namespace task_management.Enum
{
    public enum Priority
    {
        [Display(Name = "Critical")]
        Critical,
        [Display(Name = "High")]
        High,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "Low")]
        Low
    }
}
