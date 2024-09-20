using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.Implementations;
using PaySlipManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Implementations
{
    public class LeaveRequestsBALRepo:ILeaveRequestsBALRepo
    {
        public LeaveRequestsDALRepo _leaveRequestsDALRepo = new LeaveRequestsDALRepo();
        public async Task<IEnumerable<LeaveRequests>> GetAllLeaveRequestsAsync()
        {
            return await _leaveRequestsDALRepo.GetAllLeaveRequestsAsync();
        }

        public async Task<LeaveRequests> GetLeaveRequestsByidAsync(LeaveRequests _leaveRequests)
        {
            return await _leaveRequestsDALRepo.GetLeaveRequestsByidAsync(_leaveRequests);
        }
        public async Task<IEnumerable<LeaveRequests>> GetLeaveRequestsByCodeAsync(string Emp_Code)
        {
            return await _leaveRequestsDALRepo.GetLeaveRequestsByCodeAsync(Emp_Code);
        }
        public async Task<bool> CreateLeaveRequests(LeaveRequests _leaveRequests)
        {
            return await _leaveRequestsDALRepo.CreateLeaveRequests(_leaveRequests);

        }
        public async Task<bool> UpdateLeaveRequests(LeaveRequests _leaveRequests)
        {
            return await _leaveRequestsDALRepo.UpdateLeaveRequests(_leaveRequests);

        }
        public async Task<bool> DeleteLeaveRequests(LeaveRequests leaveRequests)
        {
            return await _leaveRequestsDALRepo.DeleteLeaveRequests(leaveRequests);

        }
    }
}
