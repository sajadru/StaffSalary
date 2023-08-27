namespace StaffSalary.Core.Dto.SalaryDtos
{
    public record SalaryEditContainerDto
    {
        public SalaryEditDto Data { get; set; }
        public string OverTimeCalculator { get; set; }
    }
}
