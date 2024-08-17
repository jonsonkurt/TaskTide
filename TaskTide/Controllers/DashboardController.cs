using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskTide.Data;
using TaskTide.Models;

namespace TaskTide.Models
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly TaskTideContext _context;

        public DashboardController(ILogger<DashboardController> logger, TaskTideContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Dashboard/Create
        public IActionResult Create()
        {
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new SelectListItem { Text = "Not Started", Value = "Not Started" },
                new SelectListItem { Text = "In Progress", Value = "In Progress" },
                new SelectListItem { Text = "Completed", Value = "Completed" }
            }, "Value", "Text");

            ViewBag.ImportanceOptions = new SelectList(new[]
            {
                new SelectListItem { Text = "Low Priority", Value = "Low Priority" },
                new SelectListItem { Text = "Mid Priority", Value = "Mid Priority" },
                new SelectListItem { Text = "High Priority", Value = "High Priority" }
            }, "Value", "Text");

            return View("Create");
        }

        // POST: Dashboard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskTitle,TaskDescription,DueDate,Status,Importance")] TaskList taskList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new SelectListItem { Text = "Not Started", Value = "Not Started" },
                new SelectListItem { Text = "In Progress", Value = "In Progress" },
                new SelectListItem { Text = "Completed", Value = "Completed" }
            }, "Value", "Text");

            ViewBag.ImportanceOptions = new SelectList(new[]
            {
                new SelectListItem { Text = "Low Priority", Value = "Low Priority" },
                new SelectListItem { Text = "Mid Priority", Value = "Mid Priority" },
                new SelectListItem { Text = "High Priority", Value = "High Priority"  }
            }, "Value", "Text");

            return View("Create", taskList);
        }

        // GET: Dashboard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskList = await _context.TaskList.FindAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }

            ViewBag.StatusOptions = new SelectList(new[]
            {
                new SelectListItem { Text = "Not Started", Value = "Not Started" },
                new SelectListItem { Text = "In Progress", Value = "In Progress" },
                new SelectListItem { Text = "Completed", Value = "Completed" }
            }, "Value", "Text", taskList.Status);

            ViewBag.ImportanceOptions = new SelectList(new[]
            {
                new SelectListItem { Text = "Low Priority", Value = "Low Priority" },
                new SelectListItem { Text = "Mid Priority", Value = "Mid Priority" },
                new SelectListItem { Text = "High Priority", Value = "High Priority" }
            }, "Value", "Text", taskList.Importance);

            return View(taskList);
        }

        // POST: Dashboard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskTitle,TaskDescription,DueDate,Status,Importance")] TaskList taskList)
        {
            if (id != taskList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskListExists(taskList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StatusOptions = new SelectList(new[]
            {
                new SelectListItem { Text = "Not Started", Value = "Not Started" },
                new SelectListItem { Text = "In Progress", Value = "In Progress" },
                new SelectListItem { Text = "Completed", Value = "Completed" }
            }, "Value", "Text", taskList.Status);

            ViewBag.ImportanceOptions = new SelectList(new[]
            {
                new SelectListItem { Text = "Low Priority", Value = "Low Priority" },
                new SelectListItem { Text = "Mid Priority", Value = "Mid Priority" },
                new SelectListItem { Text = "High Priority", Value = "High Priority" }
            }, "Value", "Text", taskList.Importance);

            return View(taskList);
        }

        private bool TaskListExists(int id)
        {
            throw new NotImplementedException();
        }

        // GET: Dashboard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskList = await _context.TaskList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskList == null)
            {
                return NotFound();
            }

            return View("Details", taskList);
        }

        // Get upcoming task and overall analytics
        public async Task<IActionResult> Index()
        {
            // Query the database for upcoming tasks and order them by due date in ascending order
            var upcomingTasks = await _context.TaskList
                .Where(t => t.DueDate >= DateTime.Today && t.DueDate <= DateTime.Today.AddDays(3) && t.Status != "Completed")
                .OrderBy(x => x.DueDate)
                .ToListAsync();

            // Get the number of completed tasks
            var completedCount = await _context.TaskList.CountAsync(t => t.Status == "Completed");

            // Get the number of overdue tasks
            var overdueCount = await _context.TaskList.CountAsync(t => t.DueDate < DateTime.Today && t.Status != "Completed");

            // Get the number of tasks due soon within the next 3 days
            var dueSoonCount = await _context.TaskList.CountAsync(t => t.DueDate >= DateTime.Today && t.DueDate <= DateTime.Today.AddDays(3) && t.Status != "Completed");

            // Get the total number of tasks created
            var totalCount = await _context.TaskList.CountAsync();

            // Pass the metrics and upcoming tasks to the view
            var dashboardViewModel = new DashboardViewModel
            {
                CompletedCount = completedCount,
                OverdueCount = overdueCount,
                DueSoonCount = dueSoonCount,
                TotalCount = totalCount,
                UpcomingTasks = upcomingTasks
            };

            return View(dashboardViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
