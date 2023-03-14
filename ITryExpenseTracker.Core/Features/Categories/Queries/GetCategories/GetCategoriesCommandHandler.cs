using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Features.Categories.Queries.Exceptions;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Categories.Queries.GetCategories;

public class GetCategoriesCommandHandler : IRequestHandler<GetCategoriesCommand, List<CategoryOutputModel>>
{
    ICategoryRepo _repo;

    public GetCategoriesCommandHandler(ICategoryRepo repo)
    {
        _repo = repo;
    }

    public async Task<List<CategoryOutputModel>> Handle(GetCategoriesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _repo.GetCategoriesAsync()
                    .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new GetCategoriesException(null, e);
        }
    }
}
