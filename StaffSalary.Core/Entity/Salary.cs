using StaffSalary.Core.EntityHelper;

namespace StaffSalary.Core.Entity
{
    public class Salary : BaseEntity<int>
    {
        public int PersonId { get; set; }
        public long BasicSalary { get; set; }
        public long Allowance { get; set; }
        public long Transportation { get; set; }
        public long Income { get; set; }
        public DateTime Date { get; set; }

        public Person Person { get; set; }
    }
}
