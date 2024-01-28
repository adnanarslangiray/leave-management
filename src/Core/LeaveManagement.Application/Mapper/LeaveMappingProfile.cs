using AutoMapper;
using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.Application.DTOs.Employee;
using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.Application.DTOs.Notifications;
using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Mapper;

public class LeaveMappingProfile : Profile
{
    public LeaveMappingProfile()
    {
        CreateMap<LeaveRequest, CreateLeaveRequestDto>().ReverseMap();
        CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
        CreateMap<LeaveRequest, UpdateLeaveRequestDto>().ReverseMap();


        CreateMap<Notification,CreateNotificationDto>().ReverseMap();
        CreateMap<Notification,UpdateCumulativeLeaveDto>().ReverseMap();


        CreateMap<CumulativeLeaveRequest,CreateCumulativeLeaveDto>().ReverseMap();
        CreateMap<CumulativeLeaveRequest, ListCumulativeLeaveRequestDto>().ReverseMap();

        CreateMap<ADUser, ListEmployeeDto>().ReverseMap();
    }

}