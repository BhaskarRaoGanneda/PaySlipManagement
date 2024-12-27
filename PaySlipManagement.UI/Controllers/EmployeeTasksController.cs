using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class EmployeeTasksController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;

        public EmployeeTasksController(APIServices apiServices,IOptions<ApiSettings> apiSettings)
        {
            this._apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }


        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            // Fetch all department data
            var tasks = await _apiServices.GetAllAsync<EmployeeTasksViewModel>($"{_apiSettings.EmployeeTasksEndpoint}/GetAllEmployeeTasks");

            // Calculate total number of items
            int totalItems = tasks.Count();

            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            // Ensure current page is within bounds
            int currentPage = page > totalPages ? totalPages : page;
            currentPage = currentPage < 1 ? 1 : currentPage;

            // Calculate the number of items to skip
            int skipItems = (currentPage - 1) * pageSize;

            // Get the paginated department data for the current page
            var pagedManager = tasks.Skip(skipItems).Take(pageSize).ToList();

            // Pass pagination data to the view using ViewBag
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            // ViewData for toast message
            ViewData["ToastMessage"] = "Retrieved all Employee Tasks details.";

            // Return the paginated data to the view
            return View(pagedManager);
        }
      


        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeTasksViewModel>($"{_apiSettings.EmployeeTasksEndpoint}/GetEmployeeTasksByidAsync/{id}");
            return View(response);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeTasks _tasks)
        {
            if (ModelState.IsValid)
            {
                EmployeeTasksViewModel m = new EmployeeTasksViewModel();
                m.Id = _tasks.Id;
                m.Emp_Code = _tasks.Emp_Code;
               
                return View(m);
            }
            return View(_tasks);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiServices.GetAsync<ManagerViewModel>($"{_apiSettings.EmployeeTasksEndpoint}/GetEmployeeTasksByidAsync/{id}");
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeTasks model)
        {
            if (ModelState.IsValid)
            {
                await _apiServices.PutAsync($"{_apiSettings.EmployeeTasksEndpoint}/UpdateEmployeeTasks", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeTasksViewModel>($"{_apiSettings.EmployeeTasksEndpoint}/DeleteEmployeeTasks/{id}");
            return View(response);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"{_apiSettings.EmployeeTasksEndpoint}/DeleteEmployeeTasks/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTasks(DateTime TaskDate, List<EmployeeTasks> tasks)
        {
            try
            {
                if (tasks == null || !tasks.Any())
                {
                    ModelState.AddModelError("", "No tasks submitted. Please add tasks and try again.");
                    return View("Index");
                }

                var empCode = Request.Cookies["empCode"];
                

                // Ensure tasks are submitted for the logged-in employee
                foreach (var task in tasks)
                {
                    task.Emp_Code = empCode;
                    task.TaskDate= TaskDate;
                    // Calculate the duration from FromTime to ToTime
                    if (task.TaskFrom != null && task.TaskTo != null)
                    {
                        TimeSpan duration = task.TaskTo - task.TaskFrom;
                        task.Duration = (int)duration.TotalMinutes;
                    }
                    else
                    {
                        ModelState.AddModelError("", "TaskFrom and TaskTo are required for all tasks.");
                        return View("Index");
                    }
                    // Submit tasks to the API
                    var response = await _apiServices.PostAsync<EmployeeTasks>($"{_apiSettings.EmployeeTasksEndpoint}/CreateEmployeeTasks", task);
                    //if (response != null)
                    //{
                    //    TempData["SuccessMessage"] = "Tasks submitted successfully.";
                    //    return RedirectToAction("GenaratePdf", "Employee");
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", "Failed to submit tasks. Please try again.");
                    //}
                }

                // Validate tasks (e.g., check for overlapping time entries)
                if (!ValidateTaskEntries(tasks))
                {
                    ModelState.AddModelError("", "Invalid task entries detected. Please check overlapping times or missing details.");
                    return View("Index");
                }

                //// Submit tasks to the API
                //var response = await _apiServices.PostAsync<List<EmployeeTasks>>($"{_apiSettings.EmployeeTasksEndpoint}/CreateTasks", tasks);

                //if (response != null)
                //{
                //    TempData["SuccessMessage"] = "Tasks submitted successfully.";
                //    return RedirectToAction("GenaratePdf", "Employee");
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Failed to submit tasks. Please try again.");
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            return RedirectToAction("GeneratePdf","Employee");
            //return RedirectToAction("GeneratePdf", "Employee", new { tab = "Tasks" });
        }

        private bool ValidateTaskEntries(List<EmployeeTasks> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].TaskFrom >= tasks[i].TaskTo)
                {
                    return false; // Invalid time range
                }

                for (int j = i + 1; j < tasks.Count; j++)
                {
                    if (tasks[i].TaskFrom < tasks[j].TaskTo && tasks[i].TaskTo > tasks[j].TaskFrom)
                    {
                        return false; // Overlapping time range
                    }
                }
            }

            return true;
        }
    }
}
