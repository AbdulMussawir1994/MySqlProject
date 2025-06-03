namespace MySqlProject.Models
{
    public partial class Department
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public virtual ICollection<Employee> Employees { get; private set; } = new List<Employee>();
    }
}
