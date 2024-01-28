using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Persistence.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ILeaveRequestReadRepository _leaveRequestReadRepository;
    private readonly ILeaveRequestWriteRepository _leaveRequestWriteRepository;

    public LeaveRequestService(ILeaveRequestReadRepository leaveRequestReadRepository, ILeaveRequestWriteRepository leaveRequestWriteRepository)
    {
        _leaveRequestReadRepository=leaveRequestReadRepository;
        _leaveRequestWriteRepository=leaveRequestWriteRepository;
    }

    public async Task<PagedList<List<LeaveRequest>>> GetLeaveRequestByUserId(string userId, int page, int size)
    {
        var query = _leaveRequestReadRepository.Table
            .Where(x => x.CreatedById == Guid.Parse(userId))
            //.Include(x => x.LeaveType)
            .AsNoTracking();
        var leaveRequest = await query.Skip((page - 1) * size)
        .Take(size)
        .ToListAsync();

        return new PagedList<List<LeaveRequest>>(leaveRequest, await query.CountAsync());
    }

    public async Task<PagedList<List<LeaveRequest>>> GetLeaveRequestList(int page, int size)
    {
        var query = _leaveRequestReadRepository.Table
            .AsNoTracking();
        var leaveRequest = await  query.Skip((page - 1) * size).Take(size).ToListAsync();
           
        return new PagedList<List<LeaveRequest>>(leaveRequest, await query.CountAsync());
    }

    public async Task<bool> UpdateLeaveRequestWorkflowStatusByUserId(Guid UserId, WorkflowStatusEnum workflowStatus, LeaveTypeEnum leaveType)
    {
        //var UpdateWorkflowStatus =  _leaveRequestWriteRepository.Table.FromSqlRaw("UPDATE LeaveRequests SET WorkflowStatus = {0}, LastModifiedAt = {2} WHERE Id = {3}", (int)workflowStatus, DateTime.UtcNow, leaveRequestId); 
        //return await UpdateWorkflowStatus.AnyAsync();
        var leaveRequest = await _leaveRequestReadRepository
            .Table.FirstOrDefaultAsync(x => x.CreatedById == UserId && x.LeaveType == leaveType && (x.WorkflowStatus == WorkflowStatusEnum.None || x.WorkflowStatus == WorkflowStatusEnum.Pending));
        if (leaveRequest == null)
            return false;
        leaveRequest.WorkflowStatus = workflowStatus;
        _leaveRequestWriteRepository.Update(leaveRequest);

        return await _leaveRequestWriteRepository.SaveAsync() > 0;
    }
}