namespace MySqlProject.DTOs;

public readonly record struct CreateEmployeeDto(
string EmployeeName,
decimal Salary,
string HireDate,
string DepartmentId
    );
