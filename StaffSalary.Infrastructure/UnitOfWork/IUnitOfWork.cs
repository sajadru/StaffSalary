using StaffSalary.Core.Contract.IRepositories;

namespace StaffSalary.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ISalaryRepository SalaryRepository { get; }

        Task CommitAsync();
    }
}
