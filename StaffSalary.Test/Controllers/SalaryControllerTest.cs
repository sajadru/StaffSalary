using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StaffSalary.API.Controllers;
using StaffSalary.Core.Dto.SalaryDtos;
using StaffSalary.Core.Entity;
using StaffSalary.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StaffSalary.Test.Controllers
{
    internal class SalaryControllerTest
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        public SalaryControllerTest()
        {
            unitOfWork = new Mock<IUnitOfWork>();
        }

        [Theory]
        [InlineData(1, 1)]
        public async Task GetById_SalaryDto_Exists(int id,int personId)
        {
            //arrange
            unitOfWork.Setup(c => c.SalaryRepository.GetAsync(id,personId))
                .ReturnsAsync(GetAll().FirstOrDefault(c => c.Id == id && c.PersonId == personId));
            var controller = new SalaryController(unitOfWork.Object);

            //act
            var actionResult = await controller.GetAsync(id,personId);
            var result = actionResult?.Result as OkObjectResult;
            var data = result?.Value as SalaryDto;

            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("14020101","14020701", 1)]
        public async Task GetRange_SalaryDto_Exists(string fromDate,string toDate, int personId)
        {
            //arrange
            unitOfWork.Setup(c => c.SalaryRepository.GetRangeAsync(fromDate,toDate, personId))
                .ReturnsAsync(GetAll().Where(c => c.PersonId == personId).ToList());
            var controller = new SalaryController(unitOfWork.Object);

            //act
            var actionResult = await controller.GetRangeAsync(fromDate,toDate, personId);
            var result = actionResult?.Result as OkObjectResult;
            var data = result?.Value as List<SalaryDto>;

            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task AddAsync_SalaryDto_AddedSuccessfully()
        {
            //arrange
            var dto = new SalaryAddContainerDto()
            {
                OverTimeCalculator = "CalculatorA",
                Data = new SalaryAddDto()
                {
                    FirstName= "Test",
                    LastName = "Test",
                    Allowance = 1,
                    BasicSalary = 1,
                    Transportation = 1,
                    Date = "14020101"
                }
            };
            var entity = new Salary
            {
                Id = 1,
                PersonId = 1,
                Allowance = 1,
                BasicSalary= 1,
                Income= 1,
                Transportation= 1,
                Date= new DateTime(),
                Person = new Person { Id = 1,FirstName= "Test", LastName= "Test"},
                VersionCtrl = Encoding.UTF8.GetBytes("AAAAAAAALW01")
            };
            unitOfWork.Setup(c => c.SalaryRepository.AddAsync(dto))
                .ReturnsAsync(entity);

            unitOfWork.Setup(c => c.CommitAsync())
                .Returns(Task.CompletedTask);
            var controller = new SalaryController(unitOfWork.Object);

            //act
            var actionResult = await controller.AddAsync(dto);
            var result = actionResult?.Result as OkObjectResult;
            var data = result?.Value as SalaryDto;

            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(data);
        }

        public async Task UpdateAsync_SalaryDto_UpdatedSuccessfully()
        {
            //arrange
            var dto = new SalaryEditContainerDto
            {
                OverTimeCalculator = "CalculatorB",
                Data= new SalaryEditDto() 
                {
                    Id = 1,
                    PersonId = 1,
                    Allowance=1,
                    BasicSalary= 1,
                    Transportation= 1,
                    Date = "14020501",
                    VersionCtrl = Encoding.UTF8.GetBytes("AAAAAAAALW0=")
                }
            };
            var entity = new Salary
            {
                Id = 1,
                Person = new Person { Id= 1,FirstName="test",LastName ="test"},
                PersonId= 1,
                Allowance =1,
                BasicSalary = 1,
                Income= 1,
                Transportation= 1,
                Date= new DateTime(),
                VersionCtrl = Encoding.UTF8.GetBytes("AAAAAAAALW02")
            };
            unitOfWork.Setup(c => c.SalaryRepository.UpdateAsync(dto))
                .ReturnsAsync(entity);
            unitOfWork.Setup(c => c.CommitAsync())
                .Returns(Task.CompletedTask);
            var controller = new SalaryController(unitOfWork.Object);

            //act
            var actionResult = await controller.UpdateAsync(dto);
            var result = actionResult?.Result as OkObjectResult;
            var data = result?.Value;

            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task UpdateAsync_BadRequest_UpdateFailed()
        {
            //arrange
            var dto = new SalaryEditContainerDto
            {
                OverTimeCalculator = "CalculatorB",
                Data = new SalaryEditDto()
                {
                    Id = 50,
                    PersonId = 42,
                    Allowance = 1,
                    BasicSalary = 1,
                    Transportation = 1,
                    Date = "14020501",
                    VersionCtrl = Encoding.UTF8.GetBytes("AAAAAAAALW0=")
                }
            };
            unitOfWork.Setup(c => c.SalaryRepository.UpdateAsync(dto))
                  .ReturnsAsync(() => null);
            var controller = new SalaryController(unitOfWork.Object);

            //act
            var ex = await Record.ExceptionAsync(() => controller.UpdateAsync(dto));
            var result = ex as Exception;
            //assert
            Assert.IsType<Exception>(result);
        }

        [Fact]
        public async Task DeleteAsync_Ok_DeletedSuccessfully()
        {
            //arrange
            var dto = new SalaryDeleteDto
            {
                    Id = 1,
                    PersonId = 1,
                    VersionCtrl = Encoding.UTF8.GetBytes("AAAAAAAALW0=")

            };
            unitOfWork.Setup(c => c.SalaryRepository.DeleteAsync(dto))
                .Returns(Task.CompletedTask);
            unitOfWork.Setup(c => c.CommitAsync())
                .Returns(Task.CompletedTask);
            var controller = new SalaryController(unitOfWork.Object);

            //act
            var actionResult = await controller.DeleteAsync(dto);
            var result = actionResult as OkResult;

            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_BadRequest_DeletedFailed()
        {
            //arrange
            var dto = new SalaryDeleteDto
            {
                Id = 78,
                PersonId = 87,
                VersionCtrl = Encoding.UTF8.GetBytes("AAAAAAAALW0=")

            };
            unitOfWork.Setup(c => c.SalaryRepository.DeleteAsync(dto))
                .Throws(new Exception());
            var controller = new SalaryController(unitOfWork.Object);

            //act
            Exception ex = await Record.ExceptionAsync(() => controller.DeleteAsync(dto));
            //assert
            Assert.IsType<Exception>(ex);
        }

        private List<SalaryDto> GetAll()
        {
            return new List<SalaryDto>
            {
                new SalaryDto
                {
                    Id = 1,
                    PersonId =1,
                    FirstName = "test",
                    LastName = "test",
                    Allowance = 10,
                    BasicSalary = 10,
                    Date="14020501",
                    Income = 10,
                    Transportation =10,
                    VersionCtrl = Encoding.UTF8.GetBytes("AAAAAAAALW0=")
                },
                new SalaryDto
                {
                    Id = 2,
                    PersonId =2,
                    FirstName = "test2",
                    LastName = "test2",
                    Allowance = 100,
                    BasicSalary = 100,
                    Date="14020601",
                    Income = 100,
                    Transportation =100,
                    VersionCtrl = Encoding.UTF8.GetBytes("AAAAAAAALW0=")
                }
            };
        }
    }
}
