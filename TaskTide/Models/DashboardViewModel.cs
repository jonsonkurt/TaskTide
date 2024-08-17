namespace TaskTide.Models
{
    internal class DashboardViewModel
    {
        public int CompletedCount { get; set; }
        public int OverdueCount { get; set; }
        public int DueSoonCount { get; set; }
        public int TotalCount { get; set; }
        public List<TaskList>? UpcomingTasks { get; internal set; }
    }
}