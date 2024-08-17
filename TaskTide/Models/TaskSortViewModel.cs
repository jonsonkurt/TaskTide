using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TaskTide.Models;

public class TaskSortViewModel
{
    public List<TaskList>? Tasks { get; set; }
    public SelectList? Importance { get; set; }
    public SelectList? Status { get; set; }
    public string? TaskStatus { get; set; }
    public string? TaskImportance { get; set; }
    public string? SearchString { get; set; }
}