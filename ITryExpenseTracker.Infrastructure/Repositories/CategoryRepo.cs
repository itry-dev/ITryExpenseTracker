using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Core.OutputModels;
using ITryExpenseTracker.Infrastructure.Configurations.Extensions;
using ITryExpenseTracker.Core.Exceptions;
using ITryExpenseTracker.Infrastructure.Models;

namespace ITryExpenseTracker.Infrastructure.Repositories;

public class CategoryRepo : ICategoryRepo
{
    readonly ILogger _logger;
    readonly DataContext _db;
    readonly ICategoryMapperService _mapperService;

    public CategoryRepo(DataContext db, ILoggerFactory loggerFactory, ICategoryMapperService mapperService)
    {
        _db = db;
        _logger = loggerFactory.CreateLogger<CategoryRepo>();
        _mapperService = mapperService;
    }

    #region GetCategoriesAsync
    public async Task<List<CategoryOutModel>> GetCategoriesAsync()
    {
        var result = await _db.Categories
                    .Where(w => !w.Deleted.HasValue)
                    .OrderBy(o => o.Name)
                    .ToListAsync()
                    .ConfigureAwait(false);

        return _mapperService.MapModels(result
                                        .Cast<IModelEntity>()
                                        .ToList());
    }
    #endregion

    #region Exists
    public async Task<bool> Exists(Guid categoryId)
    {
        return await _db.Categories.Where(w => w.Id == categoryId)
               .AnyAsync()
               .ConfigureAwait(false);
    }
    #endregion

    #region AddNewCategoryAsync
    public async Task AddNewCategoryAsync(string category, bool throwIfExists = true)
    {
        var row = await _db.Categories.Where(c => c.Name == category)
                    .AnyAsync()
                    .ConfigureAwait(false);

        if (row && throwIfExists)
        {
            throw new CategoryExistsException(category);
        }

        var c = new Category
        {
            Id = Guid.NewGuid(),
            Name = category,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "CategoryRepo"
        };

        _db.Categories.Add(c);


        await _db.SaveChangesAsync()
                .ConfigureAwait(false);
    }
    #endregion

    #region DeleteCategoryAsync
    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        var rows = await _db.Expenses
                        .Where(w => w.CategoryId == categoryId)
                        .CountAsync()
                        .ConfigureAwait(false);

        if (rows > 0)
        {
            throw new CategoryException($"Category with id {categoryId} cannot be deleted because it's used by {rows} expenses");
        }



        var c = await _db.Categories.Where(w => w.Id == categoryId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

        if (c == null)
        {
            return;
        }

        _db.Categories.Remove(c);

        await _db.SaveChangesAsync()
                .ConfigureAwait(false);
                    
    }
    #endregion
}
