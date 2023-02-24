using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using ITryExpenseTracker.Infrastructure.Models;

namespace ITryExpenseTracker.Mapper.MapperProfiles;

public class ProfileManager : Profile
{
    public ProfileManager()
    {
        CreateMap<Expense, ExpenseOutputModel>();
        CreateMap<Expense, ExpenseOutputSlimModel>();

        CreateMap<Category, CategoryOutModel>();
        CreateMap<Supplier, SupplierOutModel>();

        CreateMap<Category, CategoryOutModel>();

        
    }
}
