using ITryExpenseTracker.Core.ActionResults;
using ITryExpenseTracker.Core.Features.Expenses.AddNew;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpenses;
using ITryExpenseTracker.Core.Features.Suppliers.AddNew;
using ITryExpenseTracker.Core.Features.Suppliers.Queries.GetSuppliers;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITryExpenseTracker.Api.Controllers;

[ApiController]
[Route("private/api/v1/suppliers")]
[Route("api/v1/suppliers")]
[Authorize]
public class SupplierController : BaseController {
    private readonly ILogger<SupplierController> _logger;
    private readonly IMediator _mediator;

    public SupplierController(IMediator mediator, ILogger<SupplierController> logger) {
        _logger = logger;
        _mediator = mediator;
    }

    #region GetSuppliers
    [HttpGet]
    public async Task<PagedResult<SupplierOutputModel>> GetSuppliers([FromQuery] DataFilter filter) {
        var result = await _mediator
                            .Send(new GetSuppliersCommand(GetSessionUserId(), filter))
                            .ConfigureAwait(false);

        return new PagedResult<SupplierOutputModel>(200, result.Entities, result.TotalRows);
    }
    #endregion

    #region AddNew
    [HttpPost]
    public async Task<IActionResult> AddNew(SupplierInputModel model) {
        var result = await _mediator.Send(new AddNewSupplierCommand(GetSessionUserId(), model))
            .ConfigureAwait(false);

        return Ok(result);
    }
    #endregion
}

