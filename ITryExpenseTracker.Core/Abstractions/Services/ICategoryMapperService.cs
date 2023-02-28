using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Abstractions.Services;

public interface ICategoryMapperService
{
    CategoryOutputModel MapModel(IModelEntity dbEntity);

    List<CategoryOutputModel> MapModels(List<IModelEntity> entities);

}
