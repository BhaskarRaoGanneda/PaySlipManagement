using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.DapperServices.Interfaces
{
    public interface IDapperServices<T>
    {
        Task<IEnumerable<T>> ReadAllAsync(T entity);
        Task<T> ReadGetByIdAsync(T entity);
        Task<IEnumerable<T>> ReadGetByAllNullCodeAsync(T entity);
        Task<T> ReadGetByCodeAsync(T entity);
        Task<T> ReadGetByAllCodeAsync(T entity);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> ValidateAsync(T entity);
        Task<T> EmployeeByEmpCodeAsync(T entity);
    }
}
