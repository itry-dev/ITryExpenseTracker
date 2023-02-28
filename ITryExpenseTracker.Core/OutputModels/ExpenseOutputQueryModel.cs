﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.OutputModels;

public class ExpenseOutputQueryModel {
    public int TotalRows { get; set; } = 0;

    public List<ExpenseOutputModel> Entities { get; set; } = new List<ExpenseOutputModel>();

}
