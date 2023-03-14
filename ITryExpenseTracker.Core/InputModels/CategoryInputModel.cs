using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.InputModels;

public class CategoryInputModel {

    public string Name { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public Guid? Id { get; set; }
}
