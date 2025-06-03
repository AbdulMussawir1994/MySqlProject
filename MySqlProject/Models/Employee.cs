using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySqlProject.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department IDepartmentNavigation { get; set; }
    }
}
