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
        public async Task<MobileResponse<IEnumerable<GetEmployeeDto>>> GetEmployeesAsync(CancellationToken cancellationToken)
        {

            var serviceResponse = new MobileResponse<IEnumerable<GetEmployeeDto>>();

            try
            {
                var employees = await _ctx.Employees.AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken).ConfigureAwait(false);

                if (employees.Any())
                {
                    serviceResponse.Data = employees.Adapt<IEnumerable<GetEmployeeDto>>();
                    serviceResponse.Status = true;
                    serviceResponse.Message = "Employee list fetched successfully.";
                }
                else
                {
                    serviceResponse.Data = Enumerable.Empty<GetEmployeeDto>();
                    serviceResponse.Status = true;
                    serviceResponse.Message = "No employees found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<MobileResponse<GetEmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken)
        {
            var serviceResponse = new MobileResponse<GetEmployeeDto>();

            try
            {
                var newEmployee = model.Adapt<Employee>();

                await _ctx.Employees.AddAsync(newEmployee, cancellationToken); // Use AddAsync for async behavior

                var result = await _ctx.SaveChangesAsync(cancellationToken);

                if (result > 0)
                {
                    serviceResponse.Status = true;
                    serviceResponse.Message = "Employee added successfully.";
                    serviceResponse.Data = newEmployee.Adapt<GetEmployeeDto>();
                }
                else
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "Failed to add new employee.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }
        public Task<object?> UpdateEmployeeAsync(CreateEmployeeDto model, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<MobileResponse<GetEmployeeDto>> UpdateEmployeeAsync(int Id, CreateEmployeeDto model, CancellationToken token)
        {
            var serviceResponse = new MobileResponse<GetEmployeeDto>();

            try
            {
                // Fetch tracked entity for update
                var employee = await _ctx.Employees
                                         .FirstOrDefaultAsync(e => e.EmployeeId == Id, token);

                if (employee is null)
                {
                    return new MobileResponse<GetEmployeeDto>
                    {
                        Status = false,
                        Message = "Employee not found."
                    };
                }

                // Update fields (manual mapping or Mapster/AutoMapper)
                employee.FullName = model.EmployeeName;
                employee.Salary = model.Salary;
                employee.DepartmentId = model.DepartmentId;
                employee.HireDate = DateTime.UtcNow; // Only update if business logic requires

                await _ctx.SaveChangesAsync(token);

                serviceResponse.Status = true;
                serviceResponse.Message = "Employee updated successfully.";
                serviceResponse.Data = employee.Adapt<GetEmployeeDto>(); // Map to response DTO
            }
            catch (Exception ex)
            {
                // Log exception if logging enabled
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<MobileResponse<GetEmployeeDto>> GetEmployeeDetailByAsync(int Id, CancellationToken cancellationToken)
        {
            var serviceResponse = new MobileResponse<GetEmployeeDto>();

            try
            {
                var emp = await _ctx.Employees
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(x => x.EmployeeId == Id, cancellationToken);

                if (emp is null)
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "Employee not found.";
                    return serviceResponse;
                }

                serviceResponse.Data = emp.Adapt<GetEmployeeDto>();
                serviceResponse.Status = true;
                serviceResponse.Message = "Fetched successfully.";
            }
            catch (Exception ex)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<MobileResponse<bool>> DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            var serviceResponse = new MobileResponse<bool>();

            try
            {
                var emp = await _ctx.Employees.FindAsync(new object[] { id }, cancellationToken);

                if (emp is null)
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "Employee not found.";
                    return serviceResponse;
                }

                _ctx.Employees.Remove(emp);
                await _ctx.SaveChangesAsync(cancellationToken);

                serviceResponse.Status = true;
                serviceResponse.Message = "Deleted successfully.";
            }
            catch (Exception ex)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<MobileResponse<GetEmployeeDto>> PatchEmployeeAsync(int Id, string empName, CancellationToken cancellationToken)
        {
            var serviceResponse = new MobileResponse<GetEmployeeDto>();

            try
            {
                var emp = await _ctx.Employees
                                              .AsTracking()
                                              .FirstOrDefaultAsync(c => c.EmployeeId == Id, cancellationToken);

                if (emp is null)
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "Employee not found.";
                    return serviceResponse;
                }

                if (emp.FullName != empName)
                {
                    emp.FullName = empName;
                    await _ctx.SaveChangesAsync(cancellationToken);

                    serviceResponse.Status = true;
                    serviceResponse.Message = "Employee name updated successfully.";
                    serviceResponse.Data = emp.Adapt<GetEmployeeDto>();
                }
                else
                {
                    serviceResponse.Status = true;
                    serviceResponse.Message = "Employee name is already up-to-date.";
                    serviceResponse.Data = emp.Adapt<GetEmployeeDto>();
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }

    }
}
