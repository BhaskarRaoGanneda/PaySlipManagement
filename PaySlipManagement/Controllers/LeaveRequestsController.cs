using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestsBALRepo _leaveRequestsBALRepo;
        public LeaveRequestsController(ILeaveRequestsBALRepo leaveRequestsBALRepo)
        {
            _leaveRequestsBALRepo = leaveRequestsBALRepo;
        }
        [HttpGet("GetAllLeaveRequests")]
        public async Task<IEnumerable<LeaveRequests>> GetAllLeaveRequestsAsync()
        {
            return await _leaveRequestsBALRepo.GetAllLeaveRequestsAsync();
        }
        [HttpGet("GetLeaveRequestsByid/{id}")]
        public async Task<LeaveRequests> GetLeaveRequestsByidAsync(int id)
        {
            LeaveRequests _leaveRequests = new LeaveRequests();
            _leaveRequests.Id = id;
            return await _leaveRequestsBALRepo.GetLeaveRequestsByidAsync(_leaveRequests);
        }
        [HttpGet("GetLeaveRequestsByEmpCode/{Emp_Code}")]
        public async Task<IEnumerable<LeaveRequests>> GetLeaveRequestsByCodeAsync(string Emp_Code)
        {
            return await _leaveRequestsBALRepo.GetLeaveRequestsByCodeAsync(Emp_Code);
        }
        [HttpPost("CreateLeaveRequests")]
        public async Task<bool> CreateLeaveRequests(LeaveRequests _leaveRequests)
        {
            return await _leaveRequestsBALRepo.CreateLeaveRequests(_leaveRequests);

        }
        [HttpPut("UpdateLeaveRequests")]
        public async Task<bool> UpdateLeaveRequests(LeaveRequests _leaveRequests)
        {
            return await _leaveRequestsBALRepo.UpdateLeaveRequests(_leaveRequests);

        }
        [HttpGet("DeleteLeaveRequests/{id}")]
        public async Task<bool> DeleteLeaveRequests(int id)
        {
            LeaveRequests leaveRequests = new LeaveRequests();
            leaveRequests.Id = id;
            return await _leaveRequestsBALRepo.DeleteLeaveRequests(leaveRequests);
        }
    }
}
