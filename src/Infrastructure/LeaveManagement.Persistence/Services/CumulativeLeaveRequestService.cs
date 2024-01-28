using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Persistence.Contexts;
using LeaveManagement.SharedKernel.Enums;
using LeaveManagement.SharedKernel.Utilities;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Persistence.Services;


public class CumulativeLeaveRequestService : ICumulativeLeaveRequestService
{
    private readonly ICumulativeLeaveRequestReadRepository _cumulativeLeaveRequestReadRepository;
    private readonly IEmployeeReadRepository _employeeReadRepository;
    private readonly LeaveManagementDbContext _dbContext;

    public CumulativeLeaveRequestService(ICumulativeLeaveRequestReadRepository cumulativeLeaveRequestReadRepository, IEmployeeReadRepository employeeReadRepository, LeaveManagementDbContext dbContext)
    {
        _cumulativeLeaveRequestReadRepository = cumulativeLeaveRequestReadRepository;
        _employeeReadRepository=employeeReadRepository;
        _dbContext=dbContext;
    }

    public async Task<PagedList<IEnumerable<CumulativeLeaveRequest>>> GetCumulativeLeaveRequestByUserId(Guid userId, int page, int size)
    {
        var query = _cumulativeLeaveRequestReadRepository.Table.Where(x => x.UserId == userId).AsNoTracking();
        var list = query.Skip((page - 1) * size).Take(size);
        var count = await query.CountAsync();
        return new PagedList<IEnumerable<CumulativeLeaveRequest>>(list, count);

    }
    public async Task<IEnumerable<CumulativeLeaveRequest>> GetCurrentYearCumulativeLeaveRequestByUserId(Guid userId)
    {
        var query = _cumulativeLeaveRequestReadRepository.Table.Where(x => x.UserId == userId&& x.Year == DateTime.UtcNow.Year).AsNoTracking();
        return new List<CumulativeLeaveRequest>(await query.ToListAsync());

    }
    public async Task<CumulativeLeaveRequest> GetCurrentYearCumulativeAnnualLeaveRequestByUserId(Guid userId)
    {
        return await _cumulativeLeaveRequestReadRepository.Table.Include(x => x.Notifications).FirstOrDefaultAsync(x => x.UserId == userId && x.Year == DateTime.UtcNow.Year && x.LeaveType == LeaveTypeEnum.AnnualLeave);
    }
    public async Task<CumulativeLeaveRequest> GetCurrentYearCumulativeExcusedLeaveRequestByUserId(Guid userId)
    {
        return await _cumulativeLeaveRequestReadRepository.Table.Include(x => x.Notifications).FirstOrDefaultAsync(x => x.UserId == userId && x.Year == DateTime.UtcNow.Year && x.LeaveType == LeaveTypeEnum.ExcusedAbsence);
    }

    public async Task<BasePaginationResponse<IEnumerable<ListCumulativeLeaveRequestDto>>> GetAll(int page, int size)
    {

        var query = from clr in _dbContext.CumulativeLeaveRequests
                    join adu in _dbContext.ADUsers on clr.UserId equals adu.Id
                    orderby clr.Year descending
                    select new ListCumulativeLeaveRequestDto
                    {
                        FullName = adu.FullName,
                        LeaveType = clr.LeaveType.ToString(),
                        Id = clr.Id,
                        Year = clr.Year,
                        TotalHours = clr.TotalHours
                    };

        var list = query.Skip((page - 1) * size).Take(size).ToList();
        var count = await query.CountAsync();
        return new BasePaginationResponse<IEnumerable<ListCumulativeLeaveRequestDto>>(list,list != null,"",page,count,size);


    }
}