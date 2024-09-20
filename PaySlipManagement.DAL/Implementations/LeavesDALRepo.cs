using PayslipManagement.Common.Models;
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
    public class LeavesDALRepo:ILeavesDALRepo
    {
        DapperServices<Leaves> leavesRepository;
        public LeavesDALRepo()
        {
            leavesRepository = new DapperServices<Leaves>();
        }


        public async Task<IEnumerable<Leaves>> GetAllLeavesAsync()
        {
            try
            {
                var result = await leavesRepository.ReadAllAsync(new Leaves() { Id = null });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Leaves> GetLeavesByidAsync(Leaves _leaves)
        {
            try
            {
                return await leavesRepository.ReadGetByIdAsync(_leaves);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<Leaves> GetLeavesByCodeAsync(string Emp_Code)
        {
            try
            {
                Leaves l = new Leaves();
                l.Emp_Code = Emp_Code;
                DapperServices<Leaves> _leavesRepo = new DapperServices<Leaves>();
                return await _leavesRepo.ReadGetByAllCodeAsync(l);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateLeaves(Leaves _leaves)
        {
            try
            {

                if (_leaves != null)
                {
                    var employeeExists = await leavesRepository.CheckEmployeeExistsAsync(_leaves.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await leavesRepository.CreateAsync(_leaves);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateLeaves(Leaves _leaves)
        {
            try
            {
                if (_leaves != null)
                {
                    var employeeExists = await leavesRepository.CheckEmployeeExistsAsync(_leaves.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await leavesRepository.UpdateAsync(_leaves);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> DeleteLeaves(Leaves leaves)
        {
            try
            {
                if (leaves != null)
                {
                    await leavesRepository.DeleteAsync(leaves);
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
