using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTide.Models;

public class TaskList
{
    public int Id { get; set; }

    [Display(Name = "Task Title")]
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? TaskTitle { get; set; }

    [Display(Name = "Task Description")]
    [Required]
    public string? TaskDescription { get; set; }

    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public string? Status { get; set; }

    [Required]
    public string? Importance { get; set; }
}
