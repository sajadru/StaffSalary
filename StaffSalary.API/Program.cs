using Microsoft.Data.SqlClient;
using StaffSalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using StaffSalary.API.Extensions;
using StaffSalary.API.Middleware;
using StaffSalary.API.Attributes.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
connectionString = connectionStringBuilder.ToString();
builder.Services.AddDbContext<StaffSalaryDbContext>(o =>
{
    o.UseSqlServer(connectionString);
});

var apiKey = builder.Configuration.GetSection(nameof(APIKey)).Get<APIKey>();
builder.Services.AddSingleton(apiKey);
builder.Services.DependencyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandler>();

app.Run();
