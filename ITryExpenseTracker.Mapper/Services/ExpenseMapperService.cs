using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Mapper.Services;

public class ExpenseMapperService : IExpenseMapperService
{
    IMapper _mapper;

    public ExpenseMapperService(IMapper mapper) => _mapper = mapper;


    #region MapModel
    public ExpenseOutputModel MapModel(IModelEntity dbEntity)
    {        
        return _mapper.Map<ExpenseOutputModel>(dbEntity);
    }
    #endregion

    #region MapModels
    public ExpenseOutputQueryModel MapModels(List<IModelEntity> entities)
    {
        var mapped = _mapper.Map<List<IModelEntity>, List<ExpenseOutputModel>>(entities);

        return new ExpenseOutputQueryModel
        {
            Entities = mapped
        };
    }
    #endregion

    #region MapSlimExpensesResultsModel

    public ExpenseOutSlimQueryModel MapSlimExpensesResultsModel(List<IModelEntity> entities)
    {
        var mapped = _mapper.Map<List<IModelEntity>, List<ExpenseOutputSlimModel>>(entities);

        return new ExpenseOutSlimQueryModel
        {
            Entities = mapped
        };
    }
    #endregion
}
