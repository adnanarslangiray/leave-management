using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Persistence.Services;

public class CumulativeLeaveRequestService : ICumulativeLeaveRequestService
{
    private readonly ICumulativeLeaveRequestReadRepository _cumulativeLeaveRequestReadRepository;

    public CumulativeLeaveRequestService(ICumulativeLeaveRequestReadRepository cumulativeLeaveRequestReadRepository)
    {
        _cumulativeLeaveRequestReadRepository = cumulativeLeaveRequestReadRepository;
    }

    public IEnumerable<LeaveManagement.Domain.Entities.CumulativeLeaveRequest> GetCumulativeLeaveRequestByUserId(Guid userId)
    {
        return _cumulativeLeaveRequestReadRepository.Table.Where(x => x.UserId == userId );
    }
    public IEnumerable<LeaveManagement.Domain.Entities.CumulativeLeaveRequest> GetCurrentYearCumulativeLeaveRequestByUserId(Guid userId)
    {
        return _cumulativeLeaveRequestReadRepository.Table.Where(x => x.UserId == userId && x.Year == DateTime.UtcNow.Year);
    }
    public async Task<CumulativeLeaveRequest> GetCurrentYearCumulativeAnnualLeaveRequestByUserId(Guid userId)
    {
        return await _cumulativeLeaveRequestReadRepository.Table.FirstOrDefaultAsync(x => x.UserId == userId && x.Year == DateTime.UtcNow.Year && x.LeaveType == LeaveTypeEnum.AnnualLeave);
    }
    public async Task<CumulativeLeaveRequest> GetCurrentYearCumulativeExcusedLeaveRequestByUserId(Guid userId)
    {
        return await _cumulativeLeaveRequestReadRepository.Table.FirstOrDefaultAsync(x => x.UserId == userId && x.Year == DateTime.UtcNow.Year && x.LeaveType == LeaveTypeEnum.ExcusedAbsence);
    }
}