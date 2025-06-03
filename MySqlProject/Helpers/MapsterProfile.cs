using Mapster;
using MySqlProject.DTOs;
using MySqlProject.Models;

namespace MySqlProject.Helpers;

public sealed class MapsterProfile : TypeAdapterConfig
{
    public MapsterProfile()
    {
        RegisterEmployeeMappings();
    }

    private void RegisterEmployeeMappings()
    {
        TypeAdapterConfig<Employee, GetEmployeeDto>.NewConfig()
            .Map(dest => dest.EmployeeId, src => src.EmployeeId.ToString().ToLowerInvariant())
            .Map(dest => dest.EmployeeName, src => src.FullName)
            .Map(dest => dest.Salary, src => src.Salary);

        TypeAdapterConfig<CreateEmployeeDto, Employee>.NewConfig()
            .Map(dest => dest.FullName, src => src.EmployeeName)
            .Map(dest => dest.Salary, src => src.Salary)
            .Map(dest => dest.HireDate, src => src.HireDate)
             .Map(dest => dest.DepartmentId, src => src.DepartmentId)
            .IgnoreNullValues(true);
    }
}
