using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Categories.Queries.GetCategory;

public class GetCategoriesCommand : IRequest<List<CategoryOutputModel>>
{
    public GetCategoriesCommand()
    {
        //per ora non c'è alcuna azione, ritorno tutto
    }
}
