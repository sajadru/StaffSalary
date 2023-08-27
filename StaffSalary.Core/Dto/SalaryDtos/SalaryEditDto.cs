namespace StaffSalary.Core.Dto.SalaryDtos
{
    public record SalaryEditDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public long BasicSalary { get; set; }
        public long Allowance { get; set; }
        public long Transportation { get; set; }
        public string Date { get; set; }
        public byte[] VersionCtrl { get; set; }
    }
}
