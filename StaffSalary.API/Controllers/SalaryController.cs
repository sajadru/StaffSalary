using Microsoft.AspNetCore.Mvc;
using StaffSalary.API.Extensions;
using StaffSalary.Core.Dto.SalaryDtos;
using StaffSalary.Core.Entity;
using StaffSalary.Infrastructure.UnitOfWork;
using System.Globalization;

namespace StaffSalary.API.Controllers
{
    [Route("{format}/[controller]")]
    [ApiController]
    [ApiKey]
    public class SalaryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SalaryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<SalaryDto>> AddAsync([FromBody] SalaryAddContainerDto dto)
        {
            var entity = await _unitOfWork.SalaryRepository.AddAsync(dto);
            await _unitOfWork.CommitAsync();
            
            return Ok(entity.ChangeToDto());
        }

        [HttpPut]
        public async Task<ActionResult<SalaryDto>> UpdateAsync([FromBody] SalaryEditContainerDto dto)
        {
            var entity = await _unitOfWork.SalaryRepository.UpdateAsync(dto);
            await _unitOfWork.CommitAsync();

            return Ok(entity.ChangeToDto());
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync([FromBody] SalaryDeleteDto dto)
        {
            await _unitOfWork.SalaryRepository.DeleteAsync(dto);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<SalaryDto>> GetAsync([FromQuery]int id, [FromQuery] int personId)
        {
            var dto = await _unitOfWork.SalaryRepository.GetAsync(id,personId);
            return Ok(dto);
        }

        [HttpGet("GetRange")]
        public async Task<ActionResult<SalaryDto>> GetRangeAsync([FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] int personId)
        {
            var dto = await _unitOfWork.SalaryRepository.GetRangeAsync(fromDate,toDate, personId);
            return Ok(dto);
        }

    }
}