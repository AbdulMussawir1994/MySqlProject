using Microsoft.AspNetCore.Mvc;
using MySqlProject.DTOs;
using MySqlProject.Helpers;
using MySqlProject.ServiceLayer.Interface;

namespace MySqlProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

            if (!response.Status)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateNewEmployee([FromBody] CreateEmployeeDto model, CancellationToken token)
        {
            if (model is null)
                return BadRequest(new MobileResponse<bool> { Status = false, Message = ApiResponse.InvalidInput });

            var response = await _employeeLayer.CreateEmployeeAsync(model, token);

            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut(template: "Update/{Id}")]
        public async Task<ActionResult> UpdateEmployee(int Id, [FromBody] CreateEmployeeDto model, CancellationToken token)
        {
            if (model is null)
                return BadRequest(new MobileResponse<bool> { Status = false, Message = ApiResponse.InvalidInput });

            var response = await _employeeLayer.UpdateEmployeeAsync(Id, model, token);

            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("GetEmployee/{Id}")]
        public async Task<ActionResult> GetEmployeeByIdAsync(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ApiResponse.InvalidInput);
            }

            var response = await _employeeLayer.GetEmployeeDetailByAsync(Id, cancellationToken);

            if (!response.Status)
            {
                //_logger.LogWarning("GetEmployeeByIdAsync failed for Id = {Id}. Response: {@Response}", Id, response);
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            return Ok(response);

        }

        [HttpDelete("Delete/{Id}")]
        public async Task<ActionResult> DeleteEmployeeAsync(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ApiResponse.InvalidInput);
            }

            var response = await _employeeLayer.DeleteByIdAsync(Id, cancellationToken);

            if (!response.Status)
            {
                //_logger.LogWarning("DeleteEmployee failed: {@Response}", response);
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("PatchEmployee/{Id}")]
        public async Task<ActionResult> EmployeeNameUpdate(int Id, string empName, CancellationToken cancellationToken)
        {
            if (Id <= 0 || empName is null)
            {
                return BadRequest(new MobileResponse<bool> { Message = ApiResponse.InvalidInput, Status = false });
            }

            var response = await _employeeLayer.PatchEmployeeAsync(Id, empName, cancellationToken);

            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            return Ok(response);
        }
    }
}
