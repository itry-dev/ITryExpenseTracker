using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Abstractions;
public interface ISupplierRepo {

    Task<SupplierOutputQueryModel> GetSuppliersAsync(string userId, DataFilter filter);

    Task UpdateAsync(string userId, SupplierInputModel model);

    Task<SupplierOutputModel> AddNewAsync(string userId, SupplierInputModel model);

    Task<SupplierOutputModel?> GetSupplierAsync(string userId, Guid id);

    Task<bool> Exists(Guid id, string userId);

    /// <summary>
    /// Delete the supplieri identified by its id if it's not linked to any user expenses.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ITryExpenseTracker.Core.Exceptions.SupplierInUseException"></exception>
    Task DeleteAsync(Guid id, string userId);
}

