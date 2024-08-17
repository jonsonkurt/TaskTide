using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using TaskTide.Data;
using TaskTide.Models;

namespace TaskTide.Models
{
    public class TaskArchiveController : Controller
    {
        private readonly TaskTideContext _context;

        public TaskArchiveController(TaskTideContext context)
        {
            _context = context;
        }

        // GET: TaskArchive
        public async Task<IActionResult> Index()
        {
            // Query the database for completed tasks and order them by due date in descending order
            var completedTasks = await _context.TaskList
                                            .Where(t => t.Status == "Completed")
                                            .OrderByDescending(x => x.DueDate)
                                            .ToListAsync();

            return View(completedTasks);
        }

        // GET: TaskArchive/Details/5
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

            return View(taskList);
        }

        // GET: TaskLists/Create
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

        // POST: TaskLists/Create
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
                return RedirectToAction("Index", "OngoingTasks"); // Redirects to Ongoing Tasks page after adding of task
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

        // GET: TaskLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(taskList);
        }

        // POST: TaskArchive/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the task by id
            var task = await _context.TaskList.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            // Remove the task from the database
            _context.TaskList.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
