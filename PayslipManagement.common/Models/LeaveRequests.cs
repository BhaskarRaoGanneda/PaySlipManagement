using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.Common.Models
{
    public class LeaveRequests
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
    }
}
