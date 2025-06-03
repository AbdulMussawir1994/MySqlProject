using MySqlProject.DTOs;
using MySqlProject.Helpers;

namespace MySqlProject.ServiceLayer.Interface
{
    public interface IEmployeeLayer
    {
        Task<MobileResponse<bool>> DeleteByIdAsync(int id, CancellationToken cancellationToken);
        Task<MobileResponse<GetEmployeeDto>> GetEmployeeDetailByAsync(int id, CancellationToken cancellationToken);
        Task<MobileResponse<IEnumerable<GetEmployeeDto>>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<MobileResponse<GetEmployeeDto>> UpdateEmployeeAsync(int id, CreateEmployeeDto model, CancellationToken token);
        Task<MobileResponse<GetEmployeeDto>> PatchEmployeeAsync(int id, string empName, CancellationToken cancellationToken);
        Task<MobileResponse<GetEmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken);
    }
}
