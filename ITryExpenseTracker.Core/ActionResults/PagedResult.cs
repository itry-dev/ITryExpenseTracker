using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.ActionResults;

[SwaggerSchema(Required = new[] { "Description" })]
public class PagedResult<T> : IActionResult
{
    [SwaggerSchema("Contains the result", ReadOnly = true)]
    public List<T> Entities { get; private set; }

    [SwaggerSchema("Contains the http status code", ReadOnly = true)]
    public int Status { get; private set; }
    
    [SwaggerSchema("The total row amount not filtered", ReadOnly = true)]
    public int TotalCount { get; private set; }

    
    public PagedResult(int status, List<T> entities, int totalCount)
    {
        Status = status;
        Entities = entities;
        TotalCount = totalCount;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.Headers.Add("X-Total-Count", TotalCount.ToString());

        var objectResult = new ObjectResult(Entities)
        {
            StatusCode = Status
        };

        await objectResult.ExecuteResultAsync(context)
                .ConfigureAwait(false);
    }
}
