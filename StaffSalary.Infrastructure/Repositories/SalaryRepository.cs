using Microsoft.EntityFrameworkCore;
using OvertimePolicies.Calc;
using StaffSalary.Core.Contract.IRepositories;
using StaffSalary.Core.Entity;
using StaffSalary.Infrastructure.Context;
using System.Data;
using System.Globalization;
using Dapper;
using static System.Net.Mime.MediaTypeNames;
using StaffSalary.Core.Dto.SalaryDtos;

namespace StaffSalary.Infrastructure.Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        private StaffSalaryDbContext _context;

        public SalaryRepository(StaffSalaryDbContext context)
        {
            _context = context;
        }

        public async Task<Salary> AddAsync(SalaryAddContainerDto dto)
        {
           var person = FindPersonAsync(dto.Data.FirstName, dto.Data.LastName);
            object?[] objInput = new object?[] { (object?)(dto.Data.BasicSalary + dto.Data.Allowance) };
            var pc = new PersianCalendar();
            var entity = new Salary()
            {
                Allowance = dto.Data.Allowance,
                BasicSalary = dto.Data.BasicSalary,
                Date = new DateTime(Convert.ToInt32(dto.Data.Date.Substring(0, 4)), Convert.ToInt32(dto.Data.Date.Substring(3, 2)), Convert.ToInt32(dto.Data.Date.Substring(5, 2)), pc),
                Transportation = dto.Data.Transportation,
                Income = Convert.ToInt64(typeof(Calculator).GetMethod(dto.OverTimeCalculator)?.Invoke(new Calculator(), objInput))
            };
            if(person != null)
                entity.PersonId = person.Id;
            else
                entity.Person = new Person()
                {
                    FirstName = dto.Data.FirstName,
                    LastName = dto.Data.LastName,
                };

           await _context.Salaries.AddAsync(entity);
            return entity;
        }

        private async Task<Person> FindPersonAsync(string firstName,string lastName)
        {
           return await _context.People.FirstOrDefaultAsync(x=>x.FirstName == firstName && x.LastName == lastName);
        }

        public async Task<Salary> UpdateAsync(SalaryEditContainerDto dto)
        {
            var entity = await _context.Salaries.Include(x=>x.Person).FirstOrDefaultAsync(x=>x.Id == dto.Data.Id && x.PersonId == dto.Data.PersonId);
            if (entity == null)
                throw new Exception("Not Found");

            if (!entity.VersionCtrl.SequenceEqual(dto.Data.VersionCtrl))
                throw new Exception("Salary Changed");

            object?[] objInput = new object?[] { (object?)(dto.Data.BasicSalary + dto.Data.Allowance) };
            var pc = new PersianCalendar();

            entity.Allowance = dto.Data.Allowance;
            entity.BasicSalary = dto.Data.BasicSalary;
            entity.Transportation = dto.Data.Transportation;
            entity.Date = new DateTime(Convert.ToInt32(dto.Data.Date.Substring(0, 4)), Convert.ToInt32(dto.Data.Date.Substring(3, 2)), Convert.ToInt32(dto.Data.Date.Substring(5, 2)), pc);
            entity.Income = Convert.ToInt64(typeof(Calculator).GetMethod(dto.OverTimeCalculator)?.Invoke(new Calculator(), objInput));
            
            return entity;
        }

        public async Task DeleteAsync(SalaryDeleteDto dto)
        {
            var entity = await _context.Salaries.Include(x => x.Person).FirstOrDefaultAsync(x => x.Id == dto.Id && x.PersonId == dto.PersonId);
            if (entity == null)
                throw new Exception("Not Found");

            if (!entity.VersionCtrl.SequenceEqual(dto.VersionCtrl))
                throw new Exception("Salary Changed");

             _context.Salaries.Remove(entity);
        }

        public async Task<SalaryDto> GetAsync(int id,int personId)
        {
            var cnn = _context.Database.GetDbConnection();
            var query = " select top 1 s.Id,s.PersonId,s.BasicSalary,s.Allowance,s.Transportation,s.Income,format(s.Date , 'yyyyMMdd', 'fa-ir'),s.VersionCtrl,p.FirstName,p.LastName from Salary s " +
                " inner join person p on p.Id = s.PersonId " +
            $"where s.PersonId = {personId} and s.Id = {id}";

            return await cnn.ExecuteScalarAsync<SalaryDto>(query, commandType: CommandType.Text);
        }

        public async Task<List<SalaryDto>> GetRangeAsync(string fromDate,string toDate, int personId)
        {
            var pc = new PersianCalendar();
            var from = new DateTime(Convert.ToInt32(fromDate.Substring(0, 4)), Convert.ToInt32(fromDate.Substring(3, 2)), Convert.ToInt32(fromDate.Substring(5, 2)), pc);
            var to = new DateTime(Convert.ToInt32(toDate.Substring(0, 4)), Convert.ToInt32(toDate.Substring(3, 2)), Convert.ToInt32(toDate.Substring(5, 2)), pc);
            var cnn = _context.Database.GetDbConnection();
            var query = " select s.Id,s.PersonId,s.BasicSalary,s.Allowance,s.Transportation,s.Income,format(s.Date , 'yyyyMMdd', 'fa-ir'),s.VersionCtrl,p.FirstName,p.LastName from Salary s " +
                " inner join person p on p.Id = s.PersonId " +
            $"where s.PersonId = {personId} and s.Date between '{from}' and '{to}'";

            return await cnn.ExecuteScalarAsync<List<SalaryDto>>(query, commandType: CommandType.Text);
        }
    }
}
