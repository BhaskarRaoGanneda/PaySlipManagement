using PaySlipManagement.BAL.Implementations;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.DAL.Implementations;
using NPOI.SS.Formula.Functions;
using PaySlipManagement.DAL.Interfaces;
using PaySlipManagement.API.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<LoggingDbContext>(options =>
        options.UseSqlServer("Server=(LocalDb)\\LocalDBDemo;database=PayslipManagement;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true"));
builder.Services.AddTransient<IExceptionLoggerService, ExceptionLoggerService>();

builder.Services.AddScoped<IEmployeeTypeBALRepo, EmployeeTypeBALRepo>();
builder.Services.AddScoped<ILeaveRequestsBALRepo, LeaveRequestsBALRepo>();
builder.Services.AddScoped<ILeavesBALRepo, LeavesBALRepo>();
builder.Services.AddScoped<IDepartmentBALRepo, DepartmentBALRepo>();
builder.Services.AddScoped<IEmployeeBALRepo,EmployeeBALRepo>();
builder.Services.AddScoped<IAccountDetailsBALRepo, AccountDetailsBALRepo>();
builder.Services.AddScoped<ICompanyDetailsBALRepo, CompanyDetailsBALRepo>();
builder.Services.AddScoped<ISalaryBALRepo, SalaryBALRepo>();
builder.Services.AddScoped<IUserRoleBALRepo, UserRoleBALRepo>();
builder.Services.AddScoped<IRoleBALRepo, RoleBALRepo>();
builder.Services.AddScoped<ICTCDetailsBALRepo, CTCDetailsBALRepo>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
