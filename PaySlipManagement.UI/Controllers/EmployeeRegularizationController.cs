﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;
using System.Web.WebPages;

namespace PaySlipManagement.UI.Controllers
{
    public class EmployeeRegularizationController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public EmployeeRegularizationController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            _apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index(string ApprovalPerson, int page = 1, int pageSize = 8)
        {
            var empCode = Request.Cookies["empCode"];
            ApprovalPerson = empCode;
            // Fetch all leave requests
            var regularization = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.EmployeeRegularizationViewModel>($"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByManager/{ApprovalPerson}");

            // Filter only "Pending" requests
            var pendingRequests = regularization?.Where(r => r.Status == "Pending").ToList();

            // Pagination logic
            var totalPending = pendingRequests.Count();
            var totalPages = (int)Math.Ceiling(totalPending / (double)pageSize);
            var pagedPendingRequests = pendingRequests.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(pagedPendingRequests);
        }
        [HttpGet]
        public async Task<IActionResult> Index1()
        {
            // Fetch list of employees for the dropdown
            var employees = await _apiServices.GetAllAsync<EmployeeViewModel>(
                $"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            ViewBag.Employees = employees;

            ViewBag.SelectedEmpCode = null;
            ViewBag.SelectedStartDate = null;
            ViewBag.SelectedEndDate = null;

            return View(Enumerable.Empty<EmployeeRegularizationViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Index1(string Emp_Code, string StartDate, string EndDate)
        {
            var employees = await _apiServices.GetAllAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            ViewBag.Employees = employees;

            // Pass the selected values back to the view
            ViewBag.SelectedEmpCode = Emp_Code;
            ViewBag.SelectedStartDate = StartDate;
            ViewBag.SelectedEndDate = EndDate;

            // Fetch list of employees for dropdown
            var regularization = await _apiServices.GetAllAsync<EmployeeRegularizationViewModel>($"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByEmpCode/{Emp_Code}/{StartDate}/{EndDate}");

            return View(regularization);
        }
        //public async Task<IActionResult> Details(int id)
        //{
        //    var response = await _apiServices.GetAsync<EmployeeRegularizationViewModel>($"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByid/{id}");
        //    return View(response);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(EmployeeRegularization leaveRequests)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _apiServices.PostAsync<EmployeeRegularization>($"{_apiSettings.EmployeeRegularizationEndpoint}/CreateEmployeeRegularization", leaveRequests);
        //        if (response != null && response == "true")
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        return View(leaveRequests);
        //    }
        //    return View(leaveRequests);
        //}

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var response = await _apiServices.GetAsync<EmployeeRegularizationViewModel>($"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByid/{id}");
        //    return View(response);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, EmployeeRegularization model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _apiServices.PutAsync($"{_apiSettings.EmployeeRegularizationEndpoint}/UpdateEmployeeRegularization", model);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var response = await _apiServices.GetAsync<EmployeeRegularizationViewModel>($"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByid/{id}");
        //    return View(response);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var response = await _apiServices.GetAsync<bool>($"{_apiSettings.EmployeeRegularizationEndpoint}/DeleteEmployeeRegularization/{id}");
        //    if (response == true)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View("Delete");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeRegularizationViewModel>($"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByid/{id}");
            if (response != null)
            {
                var reg = response;

                if (reg.Status == "Pending")
                {
                    reg.Status = "Approved";
                    await _apiServices.PutAsync($"{_apiSettings.EmployeeRegularizationEndpoint}/UpdateEmployeeRegularization", reg);
                    

                    return Json(new { success = true, message = "Request approved successfully!" });
                }
            }

            return Json(new { success = false, message = "An error occurred while approving the request." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeRegularizationViewModel>($"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByid/{id}");
            if (response != null)
            {
                var model = response;
                if (model.Status == "Pending")
                {
                    model.Status = "Declined";
                    await _apiServices.PutAsync($"{_apiSettings.EmployeeRegularizationEndpoint}/UpdateEmployeeRegularization", model);
                    return Json(new { success = true, message = "Request canceled successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while canceling the request." });
        }

        [HttpGet]
        public async Task<IActionResult> SubmitRegularization()
        {
            var empCode = Request.Cookies["empCode"];

            // Fetch existing regularization data
            var regularizations = await _apiServices.GetAllAsync<EmployeeRegularizationViewModel>(
                $"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByCode/{empCode}");

            return View(regularizations); // Passing the list to the view
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRegularization(EmployeeRegularizationViewModel reg)
        {
            try
            { 

                var empCode = reg.Emp_Code;

                // Check if regularization for the same date already exists
                var existingRegularizations = await _apiServices.GetAllAsync<EmployeeRegularizationViewModel>(
                    $"{_apiSettings.EmployeeRegularizationEndpoint}/GetEmployeeRegularizationByCode/{empCode}");

                if (existingRegularizations.Any(r => r.EntryDate.Date == reg.EntryDate.Date))
                {
                    return Json(new { success = false, message = "Regularization for this date has already been submitted." });
                }

                // Get employee details to set manager code
                var employee = await _apiServices.GetAsync<EmployeeDetails>(
                    $"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}");

                if (employee != null)
                {
                    reg.ApprovalPerson = employee.ManagerCode;
                    reg.Status = "Pending";

                    // Save the regularization data
                    var response = await _apiServices.PostAsync<EmployeeRegularizationViewModel>(
                        $"{_apiSettings.EmployeeRegularizationEndpoint}/CreateEmployeeRegularization", reg);

                    if (!string.IsNullOrEmpty(response))
                    {
                        // Return updated data for the newly added entry
                        return Json(new
                        {
                            success = true,
                            entryDate = reg.EntryDate.ToString("yyyy-MM-dd"),
                            inTime = reg.InTime.ToString(@"hh\:mm"),
                            outTime = reg.OutTime.ToString(@"hh\:mm"),
                            workingHours = reg.WorkingHours,
                            deviationInWorkingHours = reg.DeviationInWorkingHours,
                            regularization = reg.Regularization
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }

            return Json(new { success = false, message = "An unexpected error occurred." });
        }
    }
}
