using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Infrastructure.Abstractions;
using ITryExpenseTracker.Infrastructure.Models;

namespace ITryExpenseTracker.Infrastructure.Configurations.Extensions;

public static class QueriesExtension
{
    public static IQueryable<T> FilterByUserId<T>(this IQueryable<T> query, string applicationId)
        where T : IUserOwned
    {
        return query.Where(w => w.ApplicationUserId == applicationId);
    }


}
