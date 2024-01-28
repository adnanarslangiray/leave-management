using FluentValidation;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;

namespace LeaveManagement.Application.Features.LeaveRequests.Validators;

public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestValidator()
    {
        RuleFor(p => p.LeaveRequestDto.CreatedById).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
        RuleFor(p => p.LeaveRequestDto.StartDate).NotEmpty().NotNull().WithMessage("{PropertyName} is required."); //StartDate bugünün tarihinden küçük olamaz
        RuleFor(p => p.LeaveRequestDto.StartDate).GreaterThan(p => DateTime.UtcNow).WithMessage("{PropertyName} must be greater than or equal to UtcNow.");
        RuleFor(p => p.LeaveRequestDto.EndDate).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
        RuleFor(p => p.LeaveRequestDto.EndDate).GreaterThanOrEqualTo(p => p.LeaveRequestDto.StartDate).WithMessage("{PropertyName} must be greater than or equal to StartDate.");
        RuleFor(p => p.LeaveRequestDto.LeaveType).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
    }
}