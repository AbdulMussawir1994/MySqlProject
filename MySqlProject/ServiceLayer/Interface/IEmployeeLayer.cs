using MySqlProject.DTOs;
using MySqlProject.Helpers;

namespace MySqlProject.ServiceLayer.Interface
{
    public interface IEmployeeLayer
    {
        Task<MobileResponse<IEnumerable<GetEmployeeDto>>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<MobileResponse<GetEmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken);
    }
}
