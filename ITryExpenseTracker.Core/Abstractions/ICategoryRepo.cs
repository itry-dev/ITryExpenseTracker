using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Abstractions;

public interface ICategoryRepo
{
    Task<List<CategoryOutputModel>> GetCategoriesAsync();

    Task<bool> Exists(Guid categoryId);

    Task DeleteCategoryAsync(Guid categoryId);

    Task<CategoryOutputModel> AddCategoryAsync(CategoryInputModel model, bool throwIfExists = true);

    Task<CategoryOutputModel?> UpdateCategoryAsync(CategoryInputModel model);

    Task<CategoryOutputModel?> GetCategoryByNameAsync(string name);
}
