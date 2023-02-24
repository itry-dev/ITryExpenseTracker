using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Abstractions.Services;

public interface IExpenseMapperService
{
    ExpenseOutputModel MapModel(IModelEntity dbEntity);

    ExpenseOutputQueryModel MapModels(List<IModelEntity> entities);

    ExpenseOutSlimQueryModel MapSlimExpensesResultsModel(List<IModelEntity> entities);
}
