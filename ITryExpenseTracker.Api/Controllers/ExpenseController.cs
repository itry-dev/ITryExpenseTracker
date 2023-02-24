using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITryExpenseTracker.Core.ActionResults;
using ITryExpenseTracker.Core.Features.Expenses.AddNew;
using ITryExpenseTracker.Core.Features.Expenses.Delete;
using ITryExpenseTracker.Core.Features.Expenses.Queries;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpense;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetSum;
using ITryExpenseTracker.Core.Features.Expenses.Update;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpenses;

namespace ITryExpenseTracker.Api.Controllers;

[ApiController]
[Route("private/api/v1/expenses")]
[Route("api/v1/expenses")]
[Authorize]
public class ExpenseController : BaseController
{
    private readonly ILogger<ExpenseController> _logger;
    private readonly IMediator _mediator;

    public ExpenseController(IMediator mediator, ILogger<ExpenseController> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region GetExpenses
    [HttpGet]
    public async Task<PagedResult<ExpenseOutputModel>> GetExpenses([FromQuery] DataFilter filter)
    {
        var result = await _mediator
                            .Send(new GetExpensesCommand(GetSessionUserId(),filter))
                            .ConfigureAwait(false);

        return new PagedResult<ExpenseOutputModel>(200, result.Entities, result.TotalRows);
    }
    #endregion

    #region GetExpensesSlim
    [HttpGet("slim")]
    public async Task<PagedResult<ExpenseOutputSlimModel>> GetExpensesSlim([FromQuery] DataFilter filter)
    {
        var result = await _mediator
                            .Send(new GetExpensesSlimCommand(GetSessionUserId(), filter))
                            .ConfigureAwait(false);

        return new PagedResult<ExpenseOutputSlimModel>(200, result.Entities, result.TotalRows);
    }
    #endregion

    #region GetExpense
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetExpense(Guid id)
    {
        var result = await _mediator
                            .Send(new GetExpenseCommand(GetSessionUserId(), id))
                            .ConfigureAwait(false);

        return Ok(result);
    }
    #endregion

    #region AddNew
    [HttpPost]
    public async Task<IActionResult> AddNew(ExpenseInputModel model)
    {
        var result = await _mediator.Send(new AddNewExpenseCommand(GetSessionUserId(), model))
            .ConfigureAwait(false);

        return Ok(result);
    } 
    #endregion

    #region Update
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ExpenseInputModel model)
    {
        model.Id = id;
        var result = await _mediator.Send(new UpdateExpenseCommand(GetSessionUserId(), model))
            .ConfigureAwait(false);

        return Ok(result);
    } 
    #endregion

    #region Delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteExpenseCommand(GetSessionUserId(), id))
            .ConfigureAwait(false);

        return Ok();
    }
    #endregion

    #region Get Sum
    [HttpGet("sum")]
    public async Task<IActionResult> GetSum([FromQuery] DataFilter filter)
    {
        var result = await _mediator.Send(new GetSumCommand(GetSessionUserId(), filter))
            .ConfigureAwait(false);

        return Ok(result);
    }
    #endregion

}
