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
            if (model == null)
                return BadRequest(new MobileResponse<bool> { Status = false, Message = "Invalid employee data." });

            var response = await _employeeLayer.CreateEmployeeAsync(model, token);

            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
