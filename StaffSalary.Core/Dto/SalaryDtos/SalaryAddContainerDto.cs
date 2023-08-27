namespace StaffSalary.Core.Dto.SalaryDtos
{
    public record SalaryAddContainerDto
    {
        public SalaryAddDto Data { get; set; }
        public string OverTimeCalculator { get; set; }
    }
}
