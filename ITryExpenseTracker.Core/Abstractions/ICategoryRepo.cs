using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Abstractions;

public interface ICategoryRepo
{
    Task<List<CategoryOutputModel>> GetCategoriesAsync();

    Task<bool> Exists(Guid categoryId);
    Task AddNewCategoryAsync(string category, bool throwIfExists = true);

    Task DeleteCategoryAsync(Guid categoryId);
}
