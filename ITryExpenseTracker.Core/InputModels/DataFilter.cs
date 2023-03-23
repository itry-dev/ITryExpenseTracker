using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.InputModels;

public class DataFilter
{
    public string? Q { get; set; } = null;

    public Guid? SupplierId { get; set; } = null;

    public DateTime? Date { get; set; } = null;

    public Guid? CategoryId { get; set; } = null;

    public int? Year { get; set; } = null;

    /// <summary>
    /// Year mandatory if month not zero
    /// </summary>
    public int? Month { get; set; } = null;

    /// <summary>
    /// Year and month mandatories if day not zero
    /// </summary>
    public int? Day { get; set; } = null;

    public int PageSize { get; set; }

    public int Page { get; set; }

    /// <summary>
    /// Accepted values: asc, desc (default)
    /// </summary>
    public string OrderBy { get; set; } = "desc";

}
