using FluentValidation;

namespace Application.CodeRequestRemarks.Queries.GetCodeRequestRemarksBetweenDates;

public class GetCodeRequestRemarksBetweenDatesQueryValidator : AbstractValidator<GetCodeRequestRemarksBetweenDatesQuery>
{
    public GetCodeRequestRemarksBetweenDatesQueryValidator()
    {
        RuleFor(x => x.StartDate).Must(BeAValidDate).WithMessage("Start date is required");
        RuleFor(x => x.EndDate).Must(BeAValidDate).WithMessage("End date is required");
        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start Date should be less than End Date");
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default);
    }
}
