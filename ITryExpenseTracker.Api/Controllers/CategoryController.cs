using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITryExpenseTracker.Core.Features.Categories.Queries.GetCategory;
using ITryExpenseTracker.Core.Features.Categories.Delete;

namespace ITryExpenseTracker.Api.Controllers;

[ApiController]
[Route("private/api/v1/categories")]
[Route("api/v1/categories")]
[Authorize]
public class CategoryController : BaseController
{
    private readonly ILogger<CategoryController> _logger;
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region GetCategories
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _mediator
                            .Send(new GetCategoriesCommand())
                            .ConfigureAwait(false);

        return Ok(result);
    }
    #endregion

    #region Delete
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "RequireAdministratorRole")]
    public async Task<IActionResult> Delete(Guid id)
    {
        //TODO check user exists and has permissions
        await _mediator
            .Send(new DeleteCategoryCommand(id))
            .ConfigureAwait(false);

        return Ok();
    }
    #endregion
}
