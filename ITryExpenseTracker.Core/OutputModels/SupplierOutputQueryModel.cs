using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.OutputModels;

public class SupplierOutputQueryModel {
    public int TotalRows { get; set; } = 0;

    public List<SupplierOutputModel> Entities { get; set; } = new List<SupplierOutputModel>();
}
