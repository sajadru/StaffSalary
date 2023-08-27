using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaffSalary.Core.Entity;

namespace StaffSalary.Infrastructure.EntityConfigurations
{
    internal class PersonEntityConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

            builder.Property(x => x.FirstName)
            .HasMaxLength(50);

            builder.Property(x => x.LastName)
            .HasMaxLength(50);
        }
    }
}
