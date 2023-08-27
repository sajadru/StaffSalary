using StaffSalary.Core.Contract.IRepositories;
using StaffSalary.Infrastructure.Context;
using StaffSalary.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffSalary.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StaffSalaryDbContext _dbContext;

        public UnitOfWork(StaffSalaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ISalaryRepository _salaryRepository;

        public ISalaryRepository SalaryRepository
        {
            get
            {
                if (_salaryRepository == null)
                    _salaryRepository = new SalaryRepository(_dbContext);

                return _salaryRepository;
            }
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
