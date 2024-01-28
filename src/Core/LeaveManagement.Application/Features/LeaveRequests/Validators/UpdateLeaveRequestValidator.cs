using FluentValidation;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Validators;

public class UpdateLeaveRequestValidator : AbstractValidator<UpdateLeaveRequestCommand>
{
    public UpdateLeaveRequestValidator()
    {
        RuleFor(p => p.UpdateLeaveRequestDto.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.UpdateLeaveRequestDto.WorkflowStatus)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .IsInEnum().WithMessage("{PropertyName} is not valid.");

    }
}
