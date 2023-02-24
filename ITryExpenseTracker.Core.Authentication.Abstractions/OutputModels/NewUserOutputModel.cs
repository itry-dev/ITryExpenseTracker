using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Authentication.Abstractions.OutputModels;

public class NewUserOutputModel
{
    public Guid Id { get; set; }

    public string Email { get; set; } = "";

    public string Name { get; set; } = "";

}
