using Mapster;
using Microsoft.EntityFrameworkCore;
using MySqlProject.DataContextClass;
using MySqlProject.Helpers;
using MySqlProject.ServiceLayer.Interface;
using MySqlProject.ServiceLayer.Repository;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure MySQL with high performance and reliability
builder.Services.AddDbContextPool<DataContext>(options =>
    options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(8, 0, 33)), // Adjust to your version
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
            mySqlOptions.CommandTimeout(30); // Optional timeout
        })
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // Read-only optimization
);

// Configure SQL Server with high performance and reliability
//builder.Services.AddDbContext<DataContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<DataContext>(options =>
//      options.UseSqlServer(connectionString, sqlOptions =>
//      {
//          sqlOptions.EnableRetryOnFailure(
//              maxRetryCount: 5,
//              maxRetryDelay: TimeSpan.FromSeconds(10),
//              errorNumbersToAdd: null);
//      }));

builder.Services.AddScoped<IEmployeeLayer, EmployeeLayer>();

// Swagger & Controllers
builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = false;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // ✅ Add this line
            });


//📦 Mapster Mapping
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(new MapsterProfile());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
