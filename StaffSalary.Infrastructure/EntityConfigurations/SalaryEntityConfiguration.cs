using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaffSalary.Core.Entity;

namespace StaffSalary.Infrastructure.EntityConfigurations
{
    internal class SalaryEntityConfiguration : IEntityTypeConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> builder)
        {
            builder.ToTable("Salary");

            builder.HasKey(c => new {c.Id ,c.PersonId});

            builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

            builder.HasOne(c => c.Person)
                .WithMany(c => c.Salaries)
                .HasForeignKey(c => c.PersonId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
