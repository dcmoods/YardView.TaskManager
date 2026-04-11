using FluentValidation;
using YardView.TaskManager.Server.Contracts.Tasks;

namespace YardView.TaskManager.Server.Validation;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.");
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(BeAValidStatus)
            .WithMessage("Status must be one of: todo, in_progress, done.");

    }

    private static bool BeAValidStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return false;
        return status is "todo" or "in_progress" or "done";
    }
}