using Mapster;
using Microsoft.EntityFrameworkCore;
using MySqlProject.DataContextClass;
using MySqlProject.DTOs;
using MySqlProject.Helpers;
using MySqlProject.Models;
using MySqlProject.ServiceLayer.Interface;

namespace MySqlProject.ServiceLayer.Repository
{
    public class EmployeeLayer : IEmployeeLayer
    {
        private readonly DataContext _ctx;

        public EmployeeLayer(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<MobileResponse<IEnumerable<GetEmployeeDto>>> GetEmployeesAsync(CancellationToken token)
        {
            try
            {
                var employees = await _ctx.Employees
                                          .AsNoTracking()
                                          .AsSplitQuery()
                                          .ToListAsync(token)
                                          .ConfigureAwait(false);

                if (!employees.Any())
                {
                    return MobileResponse<IEnumerable<GetEmployeeDto>>.EmptySuccess(Enumerable.Empty<GetEmployeeDto>(), "No employees found.");
                }

                var dtoList = employees.Adapt<IEnumerable<GetEmployeeDto>>();
                return MobileResponse<IEnumerable<GetEmployeeDto>>.Success(dtoList, "Employee list fetched successfully.");
            }
            catch (Exception ex)
            {
                // Consider logging the exception here with a logger
                return MobileResponse<IEnumerable<GetEmployeeDto>>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetEmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, CancellationToken token)
        {
            try
            {
                var employee = model.Adapt<Employee>();
                await _ctx.Employees.AddAsync(employee, token).ConfigureAwait(false);
                var result = await _ctx.SaveChangesAsync(token).ConfigureAwait(false);

                return result > 0
                    ? MobileResponse<GetEmployeeDto>.Success(employee.Adapt<GetEmployeeDto>(), "Employee added successfully.")
                    : MobileResponse<GetEmployeeDto>.Fail("Failed to add new employee.");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetEmployeeDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetEmployeeDto>> UpdateEmployeeAsync(int id, CreateEmployeeDto model, CancellationToken token)
        {
            try
            {
                var employee = await _ctx.Employees
                                         .FirstOrDefaultAsync(e => e.EmployeeId == id, token)
                                         .ConfigureAwait(false);

                if (employee is null)
                    return MobileResponse<GetEmployeeDto>.Fail("Employee not found.");

                employee.FullName = model.EmployeeName;
                employee.Salary = model.Salary;
                employee.DepartmentId = model.DepartmentId;
                employee.HireDate = DateTime.UtcNow; // Update only if necessary

                await _ctx.SaveChangesAsync(token).ConfigureAwait(false);

                return MobileResponse<GetEmployeeDto>.Success(employee.Adapt<GetEmployeeDto>(), "Employee updated successfully.");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetEmployeeDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetEmployeeDto>> GetEmployeeDetailByAsync(int id, CancellationToken token)
        {
            try
            {
                var emp = await _ctx.Employees
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.EmployeeId == id, token)
                                    .ConfigureAwait(false);

                return emp is null
                    ? MobileResponse<GetEmployeeDto>.Fail("Employee not found.")
                    : MobileResponse<GetEmployeeDto>.Success(emp.Adapt<GetEmployeeDto>(), "Fetched successfully.");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetEmployeeDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<bool>> DeleteByIdAsync(int id, CancellationToken token)
        {
            try
            {
                var emp = await _ctx.Employees.FindAsync(new object[] { id }, token).ConfigureAwait(false);

                if (emp is null)
                    return MobileResponse<bool>.Fail("Employee not found.");

                _ctx.Employees.Remove(emp);
                await _ctx.SaveChangesAsync(token).ConfigureAwait(false);

                return MobileResponse<bool>.Success(true, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return MobileResponse<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetEmployeeDto>> PatchEmployeeAsync(int id, string empName, CancellationToken token)
        {
            try
            {
                var emp = await _ctx.Employees
                                    .AsTracking()
                                    .FirstOrDefaultAsync(c => c.EmployeeId == id, token)
                                    .ConfigureAwait(false);

                if (emp is null)
                    return MobileResponse<GetEmployeeDto>.Fail("Employee not found.");

                if (emp.FullName != empName)
                {
                    emp.FullName = empName;
                    await _ctx.SaveChangesAsync(token).ConfigureAwait(false);
                }

                return MobileResponse<GetEmployeeDto>.Success(emp.Adapt<GetEmployeeDto>(), "Employee name updated successfully.");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetEmployeeDto>.Fail($"An error occurred: {ex.Message}");
            }
        }
    }
}
