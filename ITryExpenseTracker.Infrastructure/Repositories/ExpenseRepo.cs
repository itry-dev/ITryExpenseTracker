using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Core.Exceptions;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using ITryExpenseTracker.Infrastructure.Configurations.Extensions;
using ITryExpenseTracker.Infrastructure.Models;

namespace ITryExpenseTracker.Infrastructure.Repositories;

public class ExpenseRepo : IExpenseRepo
{
    const int MAX_PAGE_SIZE = 100;
    const int MIN_PAGE_SIZE = 10;

    private readonly string _system = "system";
    private readonly DataContext _db;
    private readonly ILogger<ExpenseRepo> _logger;
    private readonly IExpenseMapperService _mapperService;
    private readonly ICategoryRepo _categoryRepo;

    public ExpenseRepo(DataContext db, ILoggerFactory loggerFactory, ICategoryRepo categoryRepo, IExpenseMapperService mapperService)
    {
        _db= db;
        _logger = loggerFactory.CreateLogger<ExpenseRepo>();
        _mapperService = mapperService;
        _categoryRepo = categoryRepo;
    }

    #region GetExpensesSlimAsync
    public async Task<ExpenseOutputSlimQueryModel> GetExpensesSlimAsync(string userId, DataFilter filter)
    {
        var query = _getExpensesQueryByFilters(userId, filter);

        if (!await query.AnyAsync().ConfigureAwait(false))
        {
            return new ExpenseOutputSlimQueryModel();
        }

        var totalRows = await query
                            .CountAsync()
                            .ConfigureAwait(false);

        query = query
                    .AsNoTracking()
                    .GroupBy(g => g.Date.Date)
                    .Select(s => new Expense
                    {
                        Amount = s.Sum(s => s.Amount),
                        Date = s.Key
                    });

        query = _getPaginatedQuery(query, filter.Page, filter.PageSize);

        var result = await query
                        .ToListAsync()
                        .ConfigureAwait(false);

        var modelOut = _mapperService.MapSlimExpensesResultsModel(result
                                        .Cast<IModelEntity>()
                                        .ToList()
                                        );

        modelOut.TotalRows = totalRows;

        return modelOut;
    }
    #endregion

    #region GetExpensesAsync
    public async Task<ExpenseOutputQueryModel> GetExpensesAsync(string userId, DataFilter filter)
    {

        var query = _getExpensesQueryByFilters(userId, filter);

        if (!await query.AnyAsync().ConfigureAwait(false))
        {
            return new ExpenseOutputQueryModel();
        }

        var totalRows = await query
                            .CountAsync()
                            .ConfigureAwait(false);


        query = query
                .AsNoTracking()
                .Include(c => c.Category)
                .Include(c => c.Supplier);

        query = _getPaginatedQuery(query, filter.Page, filter.PageSize);
                
        var result = await query
                            .OrderByDescending(o => o.Date)
                            .ToListAsync()
                            .ConfigureAwait(false);

        var modelOut = _mapperService.MapModels(result
                                        .Cast<IModelEntity>()
                                        .ToList()
                                        );

        modelOut.TotalRows = totalRows;

        return modelOut;
    }

    
    #endregion

    #region UpdateAsync
    public async Task UpdateAsync(string userId, ExpenseInputModel model)
    {
        await _checkCategoryId(model.CategoryId);

        var dbModel = await _db.Expenses.Where(w => w.ApplicationUserId == userId && w.Id == model.Id.GetValueOrDefault())
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

        if (dbModel == null) {
            throw new ExpenseNotFoundException(model.Id.GetValueOrDefault().ToString());
        }

        dbModel.Updated = DateTime.UtcNow;
        dbModel.UpdatedBy = userId;

        _db.Entry(dbModel).CurrentValues.SetValues(model);
        
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
    #endregion

    #region AddNewAsync
    public async Task<ExpenseOutputModel> AddNewAsync(string userId, ExpenseInputModel model)
    {
        await _checkCategoryId(model.CategoryId);

        var entity = new Expense
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            ApplicationUserId = userId,
            Amount = model.Amount,
            Date = model.Date.HasValue ? model.Date.Value.ToUniversalTime() : DateTime.UtcNow,
            CategoryId = model.CategoryId,
            SupplierId = model.SupplierId.HasValue ? model.SupplierId.Value : null,
            CreatedAt = DateTime.UtcNow,
            StartDate = model.StartDate ?? null,
            EndDate = model.EndDate ?? null,
            CreatedBy = userId
        };

        _db.Expenses.Add(entity);

        await _db.SaveChangesAsync().ConfigureAwait(false);

        return await _getExpenseOutModel(entity.Id);

    }
    #endregion

    #region DeleteAsync
    public async Task DeleteAsync(string userId, Guid modelId)
    {
        var model = await _db.Expenses
                    .AsNoTracking()
                    .FilterByUserId(userId)
                    .Where(a => a.Id == modelId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

        if (model == null)
        {
            return;
        }

        _db.Expenses.Remove(model);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
    #endregion

    #region GetExpenseAsync
    public async Task<ExpenseOutputModel?> GetExpenseAsync(string userId, Guid id)
    {
        var query = _db.Expenses
                    .FilterByUserId(userId)
                    .Where(a => a.Id == id);

        if (!await query.AnyAsync())
        {
            return null;
        }

        var model = await query
                          .Include(c => c.Category)
                          .Include(c => c.Supplier)
                          .FirstOrDefaultAsync()
                          .ConfigureAwait(false);

        return _mapperService.MapModel(model);
    }
    #endregion

    #region GetSumAsync
    public async Task<ExpenseSumOutputModel> GetSumAsync(string userId, DataFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Q))
        {
            //more filters required
            if (filter.Year == 0)
            {
                throw new ExpenseSumException("To use a Q filter a year must be provided");
            }
        }

        var query = _getExpensesQueryByFilters(userId, filter, false);

        var sum = await query.Select(s => s.Amount)
                        .SumAsync()
                        .ConfigureAwait(false);

        return new ExpenseSumOutputModel { TotalAmount = sum };
                    
    }
    #endregion

    #region _getPaginatedQuery
    private IQueryable<Expense> _getPaginatedQuery(IQueryable<Expense> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize)
                        .Take(pageSize);
    }
    #endregion

    #region _getExpensesQueryByFilters
    private IQueryable<Expense> _getExpensesQueryByFilters(string userId, DataFilter filter, bool considerPaging = true)
    {
        if (considerPaging)
        {
            if (filter.PageSize <= 0) filter.PageSize = MIN_PAGE_SIZE;
            if (filter.PageSize > MAX_PAGE_SIZE) filter.PageSize = MAX_PAGE_SIZE;

            if (filter.Page <= 0) filter.Page = 1;
        }

        var query = _db.Expenses.FilterByUserId(userId);



        if (!string.IsNullOrEmpty(filter.Q))
        {
            query = query.Where(x => x.Title.Contains(filter.Q));
        }

        query = _filterByDate(query, filter);

        return query;
    }
    #endregion

    #region _filterByDate
    private IQueryable<Expense> _filterByDate(IQueryable<Expense> query, DataFilter filter)
    {
        if (filter.Year.HasValue)
        {
            if (filter.Month.HasValue)
            {
                if (filter.Day.HasValue)
                {
                    query = query.Where(x => x.Date.Year == filter.Year
                                        && x.Date.Month == filter.Month
                                        && x.Date.Day == filter.Day.Value);
                }
                else
                {
                    query = query.Where(x => x.Date.Year == filter.Year
                                        && x.Date.Month == filter.Month);
                }
            }
            else
            {
                query = query.Where(x => x.Date.Year == filter.Year);
            }
        }

        return query;
    }
    #endregion

    #region _checkCategoryId
    private async Task _checkCategoryId(Guid categoryId)
    {
        if (! await _categoryRepo.Exists(categoryId))
        {
            throw new InvalidCategoryIdException(categoryId.ToString());
        }
    }
    #endregion

    #region _getExpenseOutModel
    private async Task<ExpenseOutputModel> _getExpenseOutModel(Guid id)
    {
        var fullModel = await _db.Expenses.Where(w => w.Id == id)
                        .Include(i => i.Category)
                        .Include(i => i.Supplier)
                        .AsNoTracking()
                        .Select(s => new Expense
                        {
                            Id = s.Id,
                            Title = s.Title,
                            Category = s.Category,
                            Supplier = s.Supplier,
                            Date = s.Date,
                            Amount = s.Amount,
                            CreatedAt = s.CreatedAt,
                            Updated = s.Updated.HasValue && s.Updated.Value != DateTime.MinValue ? s.Updated.Value : null
                        })
                        .FirstAsync();

        return _mapperService.MapModel(fullModel);
    }
    #endregion
}
