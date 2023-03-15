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
using ITryExpenseTracker.Core.InputModels;

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
    public async Task<List<CategoryOutputModel>> GetCategoriesAsync()
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

    #region AddCategoryAsync
    public async Task<CategoryOutputModel> AddCategoryAsync(CategoryInputModel model, bool throwIfExists = true)
    {
        var row = await _db.Categories.Where(c => c.Name == model.Name)
                    .AnyAsync()
                    .ConfigureAwait(false);

        if (row && throwIfExists)
        {
            throw new CategoryExistsException(model.Name);
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "CategoryRepo"
        };

        _db.Categories.Add(category);


        await _db.SaveChangesAsync()
                .ConfigureAwait(false);

        return _mapperService.MapModel(category);

    }
    #endregion

    #region UpdateCategoryAsync
    public async Task<CategoryOutputModel?> UpdateCategoryAsync(CategoryInputModel model) {
        var row = await _db.Categories.Where(c => c.Id == model.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

        if (row == null) {
            return null;
        }

        row.Name = model.Name;
        
        _db.Categories.Update(row);

        await _db.SaveChangesAsync()
                .ConfigureAwait(false);

        return _mapperService.MapModel(row);

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



        var c = await _db.Categories
                .Where(w => w.Id == categoryId)
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

    #region GetCategoryByNameAsync
    public async Task<CategoryOutputModel?> GetCategoryByNameAsync(string name) {

        if (string.IsNullOrEmpty(name)) {
            throw new ArgumentNullException("category name cannot be null or empty");
        }

        var category = await _db.Categories
                        .Where(w => w.Name.ToLower() == name.ToLower())
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

        if (category != null) {
            return _mapperService.MapModel(category);
        }

        return null;
    }
    #endregion
}
