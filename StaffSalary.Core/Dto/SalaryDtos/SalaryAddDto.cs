using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffSalary.Core.Dto.SalaryDtos
{
    public record SalaryAddDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long BasicSalary { get; set; }
        public long Allowance { get; set; }
        public long Transportation { get; set; }
        public string Date { get; set; }
    }
}
