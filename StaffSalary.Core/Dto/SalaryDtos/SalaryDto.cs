namespace StaffSalary.Core.Dto.SalaryDtos
{
    public record SalaryDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long BasicSalary { get; set; }
        public long Allowance { get; set; }
        public long Transportation { get; set; }
        public long Income { get; set; }
        public string Date { get; set; }
        public byte[] VersionCtrl { get; set; }
    }
}
