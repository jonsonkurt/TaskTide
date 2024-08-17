using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskTide.Data;
using TaskTide.Models;

namespace TaskTide.Models
{
    public class OngoingTasksController : Controller
    {
        private readonly TaskTideContext _context;

        public OngoingTasksController(TaskTideContext context)
        {
            _context = context;
        }

        // GET: TaskLists
        public async Task<IActionResult> Index(string taskImportance, string searchString, string taskStatus)
        {
            // Defines the custom order of importance options
            var importanceOptions = new List<string> { "Low Priority", "Mid Priority", "High Priority" };

            // Defines the custom order of status options
            var statusOptions = new List<string> { "Not Started", "In Progress", "Completed" };

            // Create a SelectList from the custom list
            var importanceSelectList = new SelectList(importanceOptions);
            var statusSelectList = new SelectList(statusOptions);

            var tasks = from m in _context.TaskList
                        select m;

            // Apply search filter if searchString is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(s => s.TaskTitle!.Contains(searchString));
            }

            // Apply importance filter if taskImportance is provided and not "All"
            if (!string.IsNullOrEmpty(taskImportance) && taskImportance != "All")
            {
                tasks = tasks.Where(x => x.Importance == taskImportance);
            }

            // Apply status filter if taskStatus is provided and not "All"
            if (!string.IsNullOrEmpty(taskStatus) && taskStatus != "All")
            {
                tasks = tasks.Where(x => x.Status == taskStatus);
            }

            // Order tasks by due date in ascending order
            tasks = tasks.OrderBy(x => x.DueDate);

            // Exclude completed tasks
            tasks = tasks.Where(x => x.Status != "Completed");

            // Create the view model with filtered and ordered tasks, and custom importance options
            var taskImportanceVM = new TaskSortViewModel
            {
                Importance = importanceSelectList,
                Status = statusSelectList,
                Tasks = await tasks.ToListAsync()
            };

            return View(taskImportanceVM);
        }




        // GET: TaskLists/Details/5
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


        // GET: TaskLists/Edit/5
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

        // POST: TaskLists/Edit/5
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

        // POST: TaskLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskList = await _context.TaskList.FindAsync(id);
            if (taskList != null)
            {
                _context.TaskList.Remove(taskList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskListExists(int id)
        {
            return _context.TaskList.Any(e => e.Id == id);
        }
    }
}
