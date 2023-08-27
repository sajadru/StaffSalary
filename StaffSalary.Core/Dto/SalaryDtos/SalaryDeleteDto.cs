namespace StaffSalary.Core.Dto.SalaryDtos
{
    public record SalaryDeleteDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public byte[] VersionCtrl { get; set; }
    }
}
