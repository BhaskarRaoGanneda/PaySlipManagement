using PayslipManagement.Common.Models;
using System.ComponentModel.DataAnnotations;
using PaySlipManagement.UI.Utilities;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.UI.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string? Designation { get; set; }
        public String? Division { get; set; }
        public String? Email { get; set; }
        public string? PAN_Number { get; set; }
        public string MaskedPanNumber => StringHelpers.MaskPanNumber(PAN_Number);

        public string? JoiningDate { get; set; }
        public string? DepartmentName { get; set; }
    }
    public class EmployeePayPeriodsViewModel
    {
        public EmployeeDetails? Employee { get; set; }
        public List<string>? PayPeriods { get; set; }
        public LeaveRequestsViewModel? LeaveRequest { get; set; }
        public LeavesViewModel Leaves { get; set; } = new LeavesViewModel();
    }
}
