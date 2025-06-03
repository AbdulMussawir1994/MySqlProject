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
                serviceResponse.Message = "An error occurred.";
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
    }
}
