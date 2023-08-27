using StaffSalary.Core.EntityHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffSalary.Core.Entity
{
    public class Person : BaseEntity<int>
    {
        public Person()
        {
            Salaries = new List<Salary>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<Salary> Salaries { get; set; }
    }
}
