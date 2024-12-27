using PaySlipManagement.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace PaySlipManagement.UI.Models
{
    public class EmployeeTasksViewModel
    {
        public int? Id { get; set; }
        public string Emp_Code { get; set; }
        [DataType(DataType.Date)]
        public DateTime TaskDate { get; set; }
        public TimeSpan TaskFrom { get; set; }
        public TimeSpan TaskTo { get; set; }
        public string TaskDescription { get; set; }
        public int Duration { get; set; }
        public string? Status { get; set; }
        //public List<EmployeeTasks>? Tasks { get; set; }
    }
}
