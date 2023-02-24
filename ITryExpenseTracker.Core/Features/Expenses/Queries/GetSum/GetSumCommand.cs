using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetSum;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.Queries.GetSum
{
    public class GetSumCommand : IRequest<ExpenseSumOutputModel>
    {
        public DataFilter DataFilter { get; private set; }

        public string UserId { get; private set; }  

        public GetSumCommand(string userId, DataFilter dataFilter) 
        { 
            DataFilter = dataFilter;
            UserId = userId;
        }
    }
}

public sealed class GetSumCommandValidator : AbstractValidator<GetSumCommand>
{
    public GetSumCommandValidator()
    {
        RuleFor(r => r.UserId).NotNull();

        When(w => !string.IsNullOrEmpty(w.DataFilter.Q), () =>
        {
            RuleFor(x => x.DataFilter.Q)
            .MinimumLength(2);
        });

    }
}
