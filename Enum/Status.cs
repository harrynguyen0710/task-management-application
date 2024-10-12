using System.ComponentModel.DataAnnotations;

namespace task_management.Enum
{
    public enum Status
    {
        [Display(Name = "Due")]
        Due,
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "In Progress")]
        Progress,
        [Display(Name = "Todo")]
        Medium,
        [Display(Name = "Pending")]
        Pending,
    }
}
