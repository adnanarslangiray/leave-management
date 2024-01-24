using FluentValidation;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;

namespace LeaveManagement.Application.Features.LeaveRequests.Validators;

public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestValidator()
    {
        RuleFor(p => p.LeaveRequestDto.CreatedById).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
        RuleFor(p => p.LeaveRequestDto.StartDate).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
        RuleFor(p => p.LeaveRequestDto.EndDate).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
        RuleFor(p => p.LeaveRequestDto.LeaveType).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
    }
}