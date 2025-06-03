using Microsoft.AspNetCore.Mvc;
using MySqlProject.DTOs;
using MySqlProject.Helpers;
using MySqlProject.ServiceLayer.Interface;

namespace MySqlProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeLayer _employeeLayer;

        public EmployeesController(IEmployeeLayer employeeLayer)
        {
            _employeeLayer = employeeLayer;
        }

        [HttpGet("GetEmployees")]
        public async Task<ActionResult> GetEmployees(CancellationToken token)
        {
            var response = await _employeeLayer.GetEmployeesAsync(token);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateNewEmployee([FromBody] CreateEmployeeDto model, CancellationToken token)
        {
            if (model is null)
                return BadRequest(MobileResponse<bool>.Fail(ApiResponse.InvalidInput));

            var response = await _employeeLayer.CreateEmployeeAsync(model, token);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] CreateEmployeeDto model, CancellationToken token)
        {
            if (model is null)
                return BadRequest(MobileResponse<bool>.Fail(ApiResponse.InvalidInput));

            var response = await _employeeLayer.UpdateEmployeeAsync(id, model, token);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetEmployee/{id:int}")]
        public async Task<ActionResult> GetEmployeeByIdAsync(int id, CancellationToken token)
        {
            var response = await _employeeLayer.GetEmployeeDetailByAsync(id, token);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult> DeleteEmployeeAsync(int id, CancellationToken token)
        {
            var response = await _employeeLayer.DeleteByIdAsync(id, token);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("PatchEmployee/{id:int}")]
        public async Task<ActionResult> EmployeeNameUpdate(int id, [FromQuery] string empName, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(empName))
                return BadRequest(MobileResponse<bool>.Fail(ApiResponse.InvalidInput));

            var response = await _employeeLayer.PatchEmployeeAsync(id, empName, token);
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
