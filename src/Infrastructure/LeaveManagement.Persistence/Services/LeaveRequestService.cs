using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Persistence.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ILeaveRequestReadRepository _leaveRequestReadRepository;

    public LeaveRequestService(ILeaveRequestReadRepository leaveRequestReadRepository)
    {
        _leaveRequestReadRepository=leaveRequestReadRepository;
    }

    public async Task<BaseDataDto<List<LeaveRequest>>> GetLeaveRequestByUserId(string userId, int page, int size)
    {
        var query = _leaveRequestReadRepository.Table
            .Where(x => x.CreatedById == Guid.Parse(userId))
            //.Include(x => x.LeaveType)
            .AsNoTracking();
            var leaveRequest = await query.Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new BaseDataDto<List<LeaveRequest>>
        {
            Data = leaveRequest,
            TotalCount = await query.CountAsync()
        };
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestList(int page, int size)
    {
        var leaveRequests = await _leaveRequestReadRepository.Table
            .Include(x => x.LeaveType).AsNoTracking()
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        return leaveRequests;
    }
}