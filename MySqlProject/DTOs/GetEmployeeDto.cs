namespace MySqlProject.DTOs;

public readonly record struct GetEmployeeDto(
    int EmployeeId,
    string EmployeeName,
    decimal Salary,
    string HireDate,
    string DepartmentId
);
