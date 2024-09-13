using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class LeavesController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public LeavesController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            this._apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index()
        {
            var leaves = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.LeavesViewModel>($"{_apiSettings.LeavesEndpoint}/GetAllLeaves");
            return View(leaves);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiServices.GetAsync<LeavesViewModel>($"{_apiSettings.LeavesEndpoint}/GetLeavesByid/{id}");
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Leaves leaves)
        {
            if (ModelState.IsValid)
            {
                //AccountDetails accountDetails = new AccountDetails();
                //accountDetails.Id = account.Id;
                //accountDetails.Emp_Code = account.Emp_Code;
                //accountDetails.BankName = account.BankName;
                //accountDetails.BankAccountNumber = account.BankAccountNumber;
                //accountDetails.UANNumber = account.UANNumber;
                //accountDetails.PFAccountNumber = account.PFAccountNumber;
                LeavesViewModel l = new LeavesViewModel();
                l.Id = leaves.Id;
                l.Emp_Code = leaves.Emp_Code;
                l.TypeId = leaves.TypeId;
                l.TotalLeaves = leaves.TotalLeaves;
                l.LeavesAvailable = leaves.LeavesAvailable;
                l.LeavesUsed = leaves.LeavesUsed;
                var response = await _apiServices.PostAsync<Leaves>($"{_apiSettings.LeavesEndpoint}/CreateLeaves", leaves);
                if (response != null && response == "true")
                {
                    return RedirectToAction("Index");
                }
                return View(l);
            }
            return View(leaves);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiServices.GetAsync<LeavesViewModel>($"{_apiSettings.LeavesEndpoint}/GetLeavesByid/{id}");
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Leaves model)
        {
            if (ModelState.IsValid)
            {
                await _apiServices.PutAsync($"{_apiSettings.LeavesEndpoint}/UpdateLeaves", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiServices.GetAsync<LeavesViewModel>($"{_apiSettings.LeavesEndpoint}/GetLeavesByidAsync/{id}");
            return View(response);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"{_apiSettings.LeavesEndpoint}/DeleteLeaves/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }
        //[HttpPost, ActionName("SubmitRequest")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ApplyLeave(LeaveRequestsViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var employee = await _EmployeeByEmpCodeAsync(model.Emp_Code);

        //        if (employee != null)
        //        {
        //            // Ensure valid date range
        //            if (model.FromDate > model.ToDate)
        //            {
        //                ModelState.AddModelError("FromDate", "The From Date must be before the To Date.");
        //                return View(model);
        //            }

        //            // Calculate total leave days
        //            var totalLeave = (model.ToDate - model.FromDate).Value.Days + 1;
        //            model.LeavesCount = totalLeave;

        //            if (totalLeave <= 0 || totalLeave > employee.LeaveBalance)
        //            {
        //                ModelState.AddModelError("TotalLeave", "Invalid leave request amount.");
        //                return View(model);
        //            }

        //            // Update leave balance
        //            employee.LeaveBalance -= totalLeave;

        //            // Save leave request
        //            var leaveRequest = new LeaveRequest
        //            {
        //                Emp_Code = model.Emp_Code,
        //                LeaveDate = model.FromDate,
        //                LeaveDays = totalLeave,
        //                LeaveType = model.LeaveType,
        //                Reason = model.Reason,
        //                Status = "Pending"
        //            };

        //            // Add the leave request to the database
        //            _dbContext.LeaveRequests.Add(leaveRequest);

        //            // Save changes to the database
        //            await _dbContext.SaveChangesAsync();

        //            // Return confirmation view
        //            return View("LeaveConfirmation", model);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Employee not found.");
        //        }
        //    }

        //    // Return to the same view with the current model if validation fails
        //    return View(model);
        //}
    }
}
