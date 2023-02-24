using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.ActionResults;

public class PagedResult<T> : IActionResult
{
    private List<T> _entities;
    int _status;
    int _totalCount;

    public PagedResult(int status, List<T> entities, int totalCount)
    {
        _status = status;
        _entities = entities;
        _totalCount = totalCount;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.Headers.Add("X-Total-Count", _totalCount.ToString());

        var objectResult = new ObjectResult(_entities)
        {
            StatusCode = _status
        };

        await objectResult.ExecuteResultAsync(context)
                .ConfigureAwait(false);
    }
}
