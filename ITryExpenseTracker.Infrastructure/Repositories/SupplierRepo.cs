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

public class SupplierRepo : ISupplierRepo
{
    const int MAX_PAGE_SIZE = 100;
    const int MIN_PAGE_SIZE = 10;

    private readonly string _system = "system";
    private readonly DataContext _db;
    private readonly ILogger<SupplierRepo> _logger;
    private readonly ISupplierMapperService _mapperService;

    public SupplierRepo(DataContext db, ILoggerFactory loggerFactory, ISupplierMapperService mapperService)
    {
        _db= db;
        _logger = loggerFactory.CreateLogger<SupplierRepo>();
        _mapperService = mapperService;
    }

    #region GetSuppliersAsync
    public async Task<SupplierOutputQueryModel> GetSuppliersAsync(string userId, DataFilter filter)
    {

        var query = _getSuppliersQueryByFilters(userId, filter);

        if (!await query.AnyAsync().ConfigureAwait(false))
        {
            return new SupplierOutputQueryModel();
        }

        var totalRows = await query
                            .CountAsync()
                            .ConfigureAwait(false);


        query = query.AsNoTracking();

        query = _getPaginatedQuery(query, filter.Page, filter.PageSize);
                
        var result = await query.ToListAsync()
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
    public async Task UpdateAsync(string userId, SupplierInputModel model)
    {
        if (!model.Id.HasValue) {
            throw new SupplierException("supplier id cannot be null");
        }

        if (!await Exists(model.Id.Value, userId).ConfigureAwait(false)) {
            throw new SupplierNotFoundException(model.Id.GetValueOrDefault().ToString());
        }

        var dbModel = await _db.Suppliers
                        .FilterByUserId(userId)
                        .Where(w => w.Id == model.Id.Value)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

        

        _db.Entry(dbModel).CurrentValues.SetValues(model);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
    #endregion

    #region AddNewAsync
    public async Task<SupplierOutputModel> AddNewAsync(string userId, SupplierInputModel model)
    {
        var row = await _db.Suppliers
                    .FilterByUserId(userId)
                    .Where(w => w.Name == model.Name)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);  

        if (row == null) {
            row = new Supplier {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ApplicationUserId = userId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _system
            };

            _db.Suppliers.Add(row);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }        

        return await _getSupplierOutModel(row.Id);

    }
    #endregion

    #region DeleteAsync
    public async Task DeleteAsync(Guid id, string userId) {
        if (! await Exists(id, userId)) {
            return;
        }

        var isInUse = await _db.Expenses
                        .FilterByUserId(userId)
                        .Where(w => w.SupplierId == id)
                        .AnyAsync()
                        .ConfigureAwait(false);

        if (isInUse) {
            throw new SupplierInUseException(id.ToString());
        }

        _db.Suppliers.Remove(_db.Suppliers.Where(w => w.Id == id).First());

        await _db.SaveChangesAsync()
                .ConfigureAwait(false);
    }
    #endregion

    #region GetSupplierAsync
    public async Task<SupplierOutputModel?> GetSupplierAsync(string userId, Guid id)
    {       

        if (!await Exists(id, userId))
        {
            return null;
        }

        var model = await _db.Suppliers.FilterByUserId(userId)
                          .Where(w => w.Id == id)
                          .FirstOrDefaultAsync()
                          .ConfigureAwait(false);

        return _mapperService.MapModel(model);
    }
    #endregion

    #region Exists
    public async Task<bool> Exists(Guid id, string userId) {
        var row = _db.Suppliers.FilterByUserId(userId).Where(w => w.Id == id);

        return await row.AnyAsync().ConfigureAwait(false);
    }
    #endregion

    #region _getPaginatedQuery
    private IQueryable<Supplier> _getPaginatedQuery(IQueryable<Supplier> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize)
                        .Take(pageSize);
    }
    #endregion

    #region _getSuppliersQueryByFilters
    private IQueryable<Supplier> _getSuppliersQueryByFilters(string userId, DataFilter filter, bool considerPaging = true)
    {
        if (considerPaging)
        {
            if (filter.PageSize <= 0) filter.PageSize = MIN_PAGE_SIZE;
            if (filter.PageSize > MAX_PAGE_SIZE) filter.PageSize = MAX_PAGE_SIZE;

            if (filter.Page <= 0) filter.Page = 1;
        }

        var query = _db.Suppliers.Where(w => w.ApplicationUserId == userId);

        if (!string.IsNullOrEmpty(filter.Q))
        {
            query = query.Where(x => x.Name.Contains(filter.Q));
        }

        return query;
    }
    #endregion

    #region _getSupplierOutModel
    private async Task<SupplierOutputModel> _getSupplierOutModel(Guid id)
    {
        var fullModel = await _db.Suppliers.Where(w => w.Id == id)
                        .AsNoTracking()
                        .Select(s => new Supplier
                        {
                            Id = s.Id,
                            Name = s.Name                            
                        })
                        .FirstAsync();

        return _mapperService.MapModel(fullModel);
    }
    #endregion
}
