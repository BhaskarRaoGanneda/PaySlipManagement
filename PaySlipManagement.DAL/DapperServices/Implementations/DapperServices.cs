using Dapper;
using Microsoft.Data.SqlClient;
using NPOI.SS.Formula.Functions;
using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.DapperServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.DapperServices.Implementations
{
    public class DapperServices<T>: IDapperServices<T>
    {
        private string constring = "Server=(LocalDb)\\LocalDBDemo;database=PayslipManagementDB;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true";
        private SqlConnection con;
        public DapperServices()
        {
            con = new SqlConnection(constring);
            con.Open();
        }
        public async Task<IEnumerable<T>> ReadAllAsync(T entity)
        {
            try
            {
                    var sql = GetSelectStoredProcedureName(entity) + " @Id";
                    var parameters = new DynamicParameters();
                    foreach (var property in entity.GetType().GetProperties())
                    {
                        parameters.Add("@" + property.Name, property.GetValue(entity));
                    };
                    var result = await con.QueryAsync<T>(sql, parameters);
                    con.Close();
                    return result;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<T> ReadGetByIdAsync(T entity)
        {
            try
            {
                var sql = GetSelectStoredProcedureName(entity) + " @Id";
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                }
                var result = await con.QueryFirstOrDefaultAsync<T>(sql, parameters);
                con.Close();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IEnumerable<T>> ReadGetByAllNullCodeAsync(T entity)
        {
            try
            {
                var sql = GetFullSelectStoredProcedureName(entity) + " @Emp_Code";
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                };
                var result = await con.QueryAsync<T>(sql, parameters);
                con.Close();
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<T> ReadGetByAllCodeAsync(T entity)
        {
            try
            {
                var sql = GetSelectDetailsStoredProcedureName(entity) + " @Emp_Code";
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                };
                var result = await con.QueryFirstOrDefaultAsync<T>(sql, parameters);
                con.Close();
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<T> ReadGetByCodeAsync(T entity)
        {
            try
            {
                var sql = GetFullSelectStoredProcedureName(entity) + " @Emp_Code,@PaySlipForMonth";
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                }
                var result = await con.QueryFirstOrDefaultAsync<T>(sql, parameters);
                con.Close();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task CreateAsync(T entity)
        {
            try
            {
                List<string> prop = new List<string>();
                foreach (var property in entity.GetType().GetProperties())
                {
                    prop.Add(property.Name);
                }
                var pro = "";
                for (int i = 0; i < prop.Count; i++)
                {
                    if (prop[i] == "Id")
                    {
                        pro += "";
                    }
                    else
                    {
                        pro += "@" + prop[i] + ",";
                    }
                }
                pro = pro.TrimEnd(',');
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                }
                var sql = GetInsertStoredProcedureName(entity) + " " + pro;

                await con.ExecuteAsync(sql, parameters);
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task UpdateAsync(T entity)
        {

            try
            {
                List<string> prop = new List<string>();
                foreach (var property in entity.GetType().GetProperties())
                {
                    if (property.GetValue(entity) != null)
                    {
                        prop.Add(property.Name != "CreatedDate" ? property.Name : "");
                    }
                }
                var pro = "";
                for (int i = 0; i < prop.Count; i++)
                {
                    if (prop[i] == "")
                    {
                        pro += "";
                    }
                    else
                    {
                        pro += "@" + prop[i] + ",";
                    }
                }
                pro = pro.TrimEnd(',');
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                }
                var sql = GetUpdateStoredProcedureName(entity) + " " + pro;

                await con.ExecuteAsync(sql, parameters);
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task DeleteAsync(T entity)
        {
            try
            {
                var sql = GetDeleteStoredProcedureName(entity) + " @Id";
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                }
                await con.ExecuteAsync(sql, parameters);
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<T> ValidateAsync(T entity)
        {
            try
            {
                var sql = GetSelectStoredProcedureName(entity) + "sValidate @Emp_Code,@Password";
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                };
                var result = await con.QueryFirstOrDefaultAsync<T>(sql, parameters);
                con.Close();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<T> EmployeeByEmpCodeAsync(T entity)
        {
            try
            {
                var sql = GetSelectStoredProcedureName(entity) + " @Emp_Code";
                var parameters = new DynamicParameters();
                foreach (var property in entity.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(entity));
                };
                var result = await con.QueryFirstOrDefaultAsync<T>(sql, parameters);
                con.Close();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<bool> CheckEmployeeExistsAsync(string Emp_Code)
        {
            var parameters = new { Emp_Code = Emp_Code };
            var result = await con.QueryFirstOrDefaultAsync<int>(
                "sp_CheckEmployeeExists", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<bool> CheckRoleExistsAsync(int? RoleId)
        {
            var parameters = new { RoleId = RoleId };
            var result = await con.QueryFirstOrDefaultAsync<int>(
                "sp_CheckRoleExists", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }


        private string GetInsertStoredProcedureName(T entity)
        {
            return $"EXEC spInsert{entity.GetType().Name}";
        }

        private string GetSelectStoredProcedureName(T entity)
        {
            return $"EXEC spSelect{entity.GetType().Name}";
        }
        private string GetFullSelectStoredProcedureName(T entity)
        {
            return $"EXEC spSelect{entity.GetType().Name}FullDetails";
        }
        private string GetSelectDetailsStoredProcedureName(T entity)
        {
            return $"EXEC spSelect{entity.GetType().Name}CTCDetails";
        }
        private string GetSelectCodeStoredProcedureName(T entity)
        {
            return $"EXEC spSelect{entity.GetType().Name}Details";
        }
        private string GetUpdateStoredProcedureName(T entity)
        {
            return $"EXEC spUpdate{entity.GetType().Name}";
        }

        private string GetDeleteStoredProcedureName(T entity)
        {
            return $"EXEC spDelete{entity.GetType().Name}";
        }
    }
}
