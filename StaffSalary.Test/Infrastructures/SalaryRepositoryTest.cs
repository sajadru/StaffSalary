using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StaffSalary.Infrastructure.Context;
using StaffSalary.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StaffSalary.Core.Entity;
using Xunit;
using StaffSalary.Infrastructure.Repositories;
using StaffSalary.Core.Dto.SalaryDtos;

namespace StaffSalary.Test.Infrastructures
{
    internal class SalaryRepositoryTest
    {
        DbContextOptions<StaffSalaryDbContext> options = new DbContextOptionsBuilder<StaffSalaryDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;
        public SalaryRepositoryTest()
        {
            AddData().Wait();
        }

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,2)]
        public async Task GetAsync_SalaryDto_Exist(int id,int personId)
        {
            using (var context = new StaffSalaryDbContext(options))
            {
                var repo = new SalaryRepository(context);
                var result = await repo.GetAsync(id,personId);

                Assert.IsType<SalaryDto>(result);
                Assert.Equal(id, result.Id);
                Assert.Equal(personId, result.PersonId);
            }
        }

        [Theory]
        [InlineData("14020501","14021101", 1)]
        [InlineData("14020501", "14021101", 2)]
        public async Task GetRangeAsync_SalaryDto_Exist(string fromDate,string toDate, int personId)
        {
            using (var context = new StaffSalaryDbContext(options))
            {
                var repo = new SalaryRepository(context);
                var result = await repo.GetRangeAsync(fromDate,toDate, personId);

                Assert.IsType<List<SalaryDto>>(result);
                Assert.NotNull(result);

            }
        }

        [Fact]
        public async Task AddAsync_Salary_AddedSuccessfully()
        {
            using (var context = new StaffSalaryDbContext(options))
            {
                var dto = new SalaryAddContainerDto() 
                {
                    OverTimeCalculator ="CalculatorA",
                    Data = new SalaryAddDto()
                    {
                        FirstName = "test",
                        LastName = "test",
                        Allowance = 1,
                        BasicSalary = 1,
                        Transportation = 1,
                        Date = "14020501"
                    }
                };
                var repo = new SalaryRepository(context);
                var data = await repo.AddAsync(dto);

                Assert.IsType<Salary>(data);
                Assert.NotNull(data);
                Assert.NotNull(data.Income);
            }
        }

        [Fact]
        public async Task UpdateAsync_Salary_UpdatedSuccessfully()
        {
            using (var context = new StaffSalaryDbContext(options))
            {
                var dto = new SalaryEditContainerDto()
                {
                    OverTimeCalculator = "CalculatorB",
                    Data = new SalaryEditDto()
                    {
                        Allowance = 1,
                        BasicSalary = 1,
                        Transportation = 1,
                        Date = "14020501",
                        Id = 1,
                        PersonId = 1,
                        VersionCtrl = Encoding.Unicode.GetBytes("AAAAAAAALZ0=")
                    }
                };
                var repo = new SalaryRepository(context);
                var data = await repo.UpdateAsync(dto);

                Assert.IsType<Salary>(data);
                Assert.Equal(dto.Data.Id, data.Id);
                Assert.Equal(dto.Data.PersonId, data.PersonId);
            }
        }

        [Fact]
        public async Task UpdateAsync_ThrowException_NotFound()
        {
            using (var context = new StaffSalaryDbContext(options))
            {
                var dto = new SalaryEditContainerDto()
                {
                    OverTimeCalculator = "CalculatorB",
                    Data = new SalaryEditDto()
                    {
                        Allowance = 1,
                        BasicSalary = 1,
                        Transportation = 1,
                        Date = "14020501",
                        Id = 50,
                        PersonId = 1,
                        VersionCtrl = Encoding.Unicode.GetBytes("AAAAAAAALZ0=")
                    }
                };
                var repo = new SalaryRepository(context);
                Exception ex = await Record.ExceptionAsync(() => repo.UpdateAsync(dto));
                Assert.NotNull(ex);
                Assert.Equal("Not Found", ex.Message);
            }
        }

        [Fact]
        public async Task UpdateAsync_ThrowException_SequenceEqual()
        {
            using (var context = new StaffSalaryDbContext(options))
            {
                var dto = new SalaryEditContainerDto()
                {
                    OverTimeCalculator = "CalculatorB",
                    Data = new SalaryEditDto()
                    {
                        Allowance = 1,
                        BasicSalary = 1,
                        Transportation = 1,
                        Date = "14020501",
                        Id = 1,
                        PersonId = 1,
                        VersionCtrl = Encoding.Unicode.GetBytes("AAAAAAAALD0=")
                    }
                };
                var repo = new SalaryRepository(context);
                Exception ex = await Record.ExceptionAsync(() => repo.UpdateAsync(dto));
                Assert.NotNull(ex);
                Assert.Equal("Salary Changed", ex.Message);
            }
        }

        [Fact]
        public async Task DeleteAsync_Exception_DoesNotExist()
        {
            using (var context = new StaffSalaryDbContext(options))
            {
                var dto = new SalaryDeleteDto()
                {
                    Id = 10,
                    PersonId = 1,
                    VersionCtrl = Encoding.Unicode.GetBytes("AAAAAAAALZ0=")
                };
                var repo = new SalaryRepository(context);
                Exception ex = await Record.ExceptionAsync(() => repo.DeleteAsync(dto));
                Assert.NotNull(ex);
                Assert.Equal("Not Found", ex.Message);
            }
        }

        private async Task AddData()
        {
            using (var context = new StaffSalaryDbContext(options))
            {
                if (!(await context.Salaries.AnyAsync(c => c.Id == 1)))
                {
                    context.Salaries.Add(new Salary
                    {
                        Id = 1,
                        PersonId = 1,
                        Allowance = 1,
                        BasicSalary = 1,
                        Income = 1,
                        Transportation = 1,
                        Date = new DateTime(),
                        VersionCtrl = Encoding.Unicode.GetBytes("AAAAAAAALX0=")
                    });
                }
                if (!(await context.Salaries.AnyAsync(c => c.Id == 2)))
                {
                    context.Salaries.Add(new Salary
                    {
                        Id = 2,
                        PersonId = 2,
                        Allowance = 10,
                        BasicSalary = 10,
                        Income = 10,
                        Transportation = 10,
                        Date = new DateTime(),
                        VersionCtrl = Encoding.Unicode.GetBytes("AAAAAAAALZ0=")
                    });
                }
                await context.SaveChangesAsync();

            }
        }
    }
}
