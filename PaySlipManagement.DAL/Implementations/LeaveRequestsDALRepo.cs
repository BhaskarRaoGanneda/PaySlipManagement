using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.DapperServices.Implementations;
using PaySlipManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Implementations
{
    public class LeaveRequestsDALRepo: ILeaveRequestsDALRepo
    {
        DapperServices<LeaveRequests> leaveRequestsRepository;
        public LeaveRequestsDALRepo()
        {
            leaveRequestsRepository = new DapperServices<LeaveRequests>();
        }
        public async Task<IEnumerable<LeaveRequests>> GetAllLeaveRequestsAsync()
        {
            try
            {
                var result = await leaveRequestsRepository.ReadAllAsync(new LeaveRequests() { Id = null });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<LeaveRequests> GetLeaveRequestsByidAsync(LeaveRequests _leaveRequests)
        {
            try
            {
                return await leaveRequestsRepository.ReadGetByIdAsync(_leaveRequests);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<LeaveRequests>> GetLeaveRequestsByCodeAsync(string Emp_Code)
        {
            try
            {
                LeaveRequests lr = new LeaveRequests();
                lr.Emp_Code = Emp_Code;
                DapperServices<LeaveRequests> _leaveRequestsRepo = new DapperServices<LeaveRequests>();
                return await _leaveRequestsRepo.ReadGetByAllNullCodeAsync(lr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateLeaveRequests(LeaveRequests _leaveRequests)
        {
            try
            {
                var employeeExists = await leaveRequestsRepository.CheckEmployeeExistsAsync(_leaveRequests.Emp_Code);
                if (!employeeExists)
                {
                    return false; // Employee does not exist, creation cannot proceed
                }
                if (_leaveRequests != null)
                {
                    await leaveRequestsRepository.CreateAsync(_leaveRequests);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateLeaveRequests(LeaveRequests _leaveRequests)
        {
            try
            {
                if (_leaveRequests != null)
                {
                    var employeeExists = await leaveRequestsRepository.CheckEmployeeExistsAsync(_leaveRequests.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await leaveRequestsRepository.UpdateAsync(_leaveRequests);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> DeleteLeaveRequests(LeaveRequests leaveRequests)
        {
            try
            {
                if (leaveRequests != null)
                {
                    await leaveRequestsRepository.DeleteAsync(leaveRequests);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
