using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskTide.Models;

namespace TaskTide.Data
{
    public class TaskTideContext : DbContext
    {
        public TaskTideContext (DbContextOptions<TaskTideContext> options)
            : base(options)
        {
        }

        public DbSet<TaskTide.Models.TaskList> TaskList { get; set; } = default!;
    }
}
