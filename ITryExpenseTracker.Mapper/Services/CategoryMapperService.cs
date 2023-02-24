using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Mapper.Services;

public class CategoryMapperService : ICategoryMapperService
{
    IMapper _mapper;

    public CategoryMapperService(IMapper mapper) => _mapper = mapper;


    #region MapModel
    public CategoryOutModel MapModel(IModelEntity dbEntity)
    {
        return _mapper.Map<CategoryOutModel>(dbEntity);
    }
    #endregion

    #region MapModels
    public List<CategoryOutModel> MapModels(List<IModelEntity> entities)
    {
        return _mapper.Map<List<CategoryOutModel>>(entities);
    }
    #endregion
}
