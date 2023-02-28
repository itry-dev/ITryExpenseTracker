using AutoMapper;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Core.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Mapper.Services;

public class SupplierMapperService : ISupplierMapperService {
    IMapper _mapper;

    public SupplierMapperService(IMapper mapper) => _mapper = mapper;

    public SupplierOutputModel MapModel(IModelEntity dbEntity) {
        return _mapper.Map<SupplierOutputModel>(dbEntity);
    }

    public SupplierOutputQueryModel MapModels(List<IModelEntity> entities) {
        var mapped = _mapper.Map<List<IModelEntity>, List<SupplierOutputModel>>(entities);

        return new SupplierOutputQueryModel {
            Entities = mapped
        };
    }
}
