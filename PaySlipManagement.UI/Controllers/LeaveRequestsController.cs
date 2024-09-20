using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;
using System.Threading.Tasks;

namespace PaySlipManagement.UI.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public LeaveRequestsController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            _apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index()
        {
            var leaveRequests = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetAllLeaveRequests");
            return View(leaveRequests);
        }
        public async Task<IActionResult> Index1(string Emp_Code)
        {
            var empCode = Request.Cookies["empCode"];
            Emp_Code = empCode;
            var leaveRequests = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByEmpCode/{Emp_Code}");
            return View(leaveRequests);
        }
        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequests leaveRequests)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiServices.PostAsync<LeaveRequests>($"{_apiSettings.LeaveRequestsEndpoint}/CreateLeaveRequests", leaveRequests);
                if (response != null && response == "true")
                {
                    return RedirectToAction("Index");
                }
                return View(leaveRequests);
            }
            return View(leaveRequests);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveRequests model)
        {
            if (ModelState.IsValid)
            {
                await _apiServices.PutAsync($"{_apiSettings.LeaveRequestsEndpoint}/UpdateLeaveRequests", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            return View(response);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"{_apiSettings.LeaveRequestsEndpoint}/DeleteLeaveRequests/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            if (response != null)
            {
                var model = response;
                if (model.Status == "Pending")
                {
                    model.Status = "Approved";
                    var count = 0;
                    if (model.FromDate != null && model.ToDate != null)
                    {
                        count = (model.ToDate - model.FromDate).Value.Days + 1;
                    }
                    model.LeavesCount += count;
                    await _apiServices.PutAsync($"{_apiSettings.LeaveRequestsEndpoint}/UpdateLeaveRequests", model);
                    return Json(new { success = true, message = "Request approved successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while approving the request." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            if (response != null)
            {
                var model = response;
                if (model.Status == "Pending")
                {
                    model.Status = "Declined";
                    await _apiServices.PutAsync($"{_apiSettings.LeaveRequestsEndpoint}/UpdateLeaveRequests", model);
                    return Json(new { success = true, message = "Request canceled successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while canceling the request." });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyLeave(LeaveRequests leaveRequests)
        {
            var empCode = Request.Cookies["empCode"];
            leaveRequests.Emp_Code = empCode;
            leaveRequests.ApprovalPerson = "Admin";
            leaveRequests.Status = "Pending";
            var count = 0;
            if (leaveRequests.FromDate != null && leaveRequests.ToDate != null)
            {
                count = (leaveRequests.ToDate - leaveRequests.FromDate).Value.Days + 1;
            }
            leaveRequests.LeavesCount = count;
            var response = await _apiServices.PostAsync<LeaveRequests>($"{_apiSettings.LeaveRequestsEndpoint}/CreateLeaveRequests", leaveRequests);
            return RedirectToAction("Index1");
        }
    }
}