using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public SalesController(IMediator mediator)
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

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="id">The sale ID.</param>
    /// <param name="command">The sale update command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale details.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.SaleId)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "The ID in the URL does not match the ID in the request body."
            });

        var validator = new UpdateSaleCommandValidator();
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

        return Ok(new ApiResponseWithData<UpdateSaleResult>
        {
            Success = true,
            Message = "Sale updated successfully.",
            Data = result
        });
    }
}