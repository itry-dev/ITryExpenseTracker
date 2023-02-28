using FluentValidation;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Suppliers.AddNew;

public class AddNewSupplierCommand : IRequest<SupplierOutputModel> {

    public readonly SupplierInputModel InputModel;
    public readonly string UserId;

    public AddNewSupplierCommand(string userId, SupplierInputModel model) 
    { 
        UserId = userId;
        InputModel = model;
    }
}

public sealed class AddNewSupplierCommandValidator : AbstractValidator<AddNewSupplierCommand> {

    public AddNewSupplierCommandValidator() {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.InputModel.Name).NotEmpty();
    }
}
