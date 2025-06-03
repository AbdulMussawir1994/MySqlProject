using Microsoft.EntityFrameworkCore;
using MySqlProject.Models;

namespace MySqlProject.DataContextClass;

public partial class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Employee>()
            .Navigation(x => x.IDepartmentNavigation)
            .AutoInclude();


        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId);
            entity.ToTable("Department");

            entity.Property(e => e.DepartmentName)
                  .HasMaxLength(50)
                  .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);
            entity.ToTable("Employee");

            entity.Property(e => e.FullName)
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.HireDate)
                  .HasColumnType("datetime");

            entity.Property(e => e.Salary)
                  .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IDepartmentNavigation)
                  .WithMany(p => p.Employees)
                  .HasForeignKey(d => d.DepartmentId)
                  .HasConstraintName("FK_Employee_Department");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}