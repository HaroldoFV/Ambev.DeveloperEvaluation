using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sales operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SaleController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public SaleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="command">The sale creation command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale details.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(e => new ValidationErrorDetail
                    {
                        Error = e.PropertyName,
                        Detail = e.ErrorMessage
                    })
                    .ToList()
            });

        var result = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResult>
        {
            Success = true,
            Message = "Sale created successfully.",
            Data = result
        });
    }
}