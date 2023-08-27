using StaffSalary.Core.Dto.SalaryDtos;
using StaffSalary.Core.Entity;
using System.Globalization;

namespace StaffSalary.API.Extensions
{
    public static class Model
    {
        public static SalaryDto ChangeToDto(this Salary entity)
        {
            var pc = new PersianCalendar();
            return new SalaryDto()
            {
                Id = entity.Id,
                PersonId = entity.PersonId,
                FirstName = entity.Person.FirstName,
                LastName = entity.Person.LastName,
                Allowance = entity.Allowance,
                BasicSalary = entity.BasicSalary,
                Income = entity.Income,
                Transportation = entity.Transportation,
                VersionCtrl = entity.VersionCtrl,
                Date = pc.GetYear(entity.Date).ToString() + pc.GetMonth(entity.Date).ToString() + pc.GetDayOfMonth(entity.Date).ToString(),
            };
        }
    }
}
