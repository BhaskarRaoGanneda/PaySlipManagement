using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Interfaces
{
    public interface ILeaveRequestsBALRepo
    {
        Task<IEnumerable<LeaveRequests>> GetAllLeaveRequestsAsync();

        Task<LeaveRequests> GetLeaveRequestsByidAsync(LeaveRequests _leaveRequests);
        Task<IEnumerable<LeaveRequests>> GetLeaveRequestsByCodeAsync(string Emp_Code);

        Task<bool> CreateLeaveRequests(LeaveRequests _leaveRequests);
        Task<bool> UpdateLeaveRequests(LeaveRequests _leaveRequests);
        Task<bool> DeleteLeaveRequests(LeaveRequests leaveRequests);
    }
}
