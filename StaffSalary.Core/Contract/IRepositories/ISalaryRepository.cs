using StaffSalary.Core.Dto.SalaryDtos;
using StaffSalary.Core.Entity;

namespace StaffSalary.Core.Contract.IRepositories
{
    public interface ISalaryRepository
    {
        Task<Salary> AddAsync(SalaryAddContainerDto dto);
        Task<Salary> UpdateAsync(SalaryEditContainerDto dto);
        Task DeleteAsync(SalaryDeleteDto dto);
        Task<SalaryDto> GetAsync(int id, int personId);
        Task<List<SalaryDto>> GetRangeAsync(string fromDate, string toDate, int personId);
    }
}
