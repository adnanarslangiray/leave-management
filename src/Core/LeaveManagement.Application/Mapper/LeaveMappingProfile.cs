using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Mapper;

public class LeaveMappingProfile : Profile
{
    public LeaveMappingProfile()
    {
        CreateMap<LeaveRequest, CreateLeaveRequestDto>().ReverseMap();
        CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
    }

}