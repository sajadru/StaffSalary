using Microsoft.EntityFrameworkCore;
using StaffSalary.Core.Entity;
using StaffSalary.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffSalary.Infrastructure.Context
{
    public class StaffSalaryDbContext : DbContext
    {
        public StaffSalaryDbContext(DbContextOptions<StaffSalaryDbContext> options): base(options)
        {
        }

        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SalaryEntityConfiguration());
        }
    }
}
