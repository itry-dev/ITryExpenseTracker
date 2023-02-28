using ITryExpenseTracker.Core.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Abstractions.Services;

public interface ISupplierMapperService {

    SupplierOutputModel MapModel(IModelEntity dbEntity);

    SupplierOutputQueryModel MapModels(List<IModelEntity> entities);
}
