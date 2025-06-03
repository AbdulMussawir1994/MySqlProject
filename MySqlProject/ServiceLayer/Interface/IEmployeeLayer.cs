using MySqlProject.DTOs;
using MySqlProject.Helpers;

namespace MySqlProject.ServiceLayer.Interface
{
    public interface IEmployeeLayer
    {
        Task<MobileResponse<bool>> DeleteByIdAsync(int Id, CancellationToken cancellationToken);
        Task<MobileResponse<GetEmployeeDto>> GetEmployeeDetailByAsync(int Id, CancellationToken cancellationToken);
        Task<MobileResponse<IEnumerable<GetEmployeeDto>>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<MobileResponse<GetEmployeeDto>> UpdateEmployeeAsync(int Id, CreateEmployeeDto model, CancellationToken token);
        Task<MobileResponse<GetEmployeeDto>> PatchEmployeeAsync(int Id, string empName, CancellationToken cancellationToken);
        Task<MobileResponse<GetEmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken);
    }
}
