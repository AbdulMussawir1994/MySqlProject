namespace MySqlProject.DTOs;

public class CreateEmployeeDto
{
    public required string EmployeeName { get; set; } = string.Empty;
    public required decimal Salary { get; set; } = 0;
    public string HireDate { get; set; } = string.Empty;
    public required int DepartmentId { get; set; }

}
