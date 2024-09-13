using System.ComponentModel.DataAnnotations;

namespace PaySlipManagement.UI.Models
{
    public class LeaveRequestsViewModel
    {
        public int? Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? LeaveType { get; set; }
        public string? Reason { get; set; }
        [DataType(DataType.Date)]

        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]

        public DateTime? ToDate { get; set; }
        public decimal LeavesCount { get; set; }
        public string? ApprovalPerson { get; set; }
        public string? Status { get; set; }
        public decimal LeaveBalance { get; set; }
    }
}
