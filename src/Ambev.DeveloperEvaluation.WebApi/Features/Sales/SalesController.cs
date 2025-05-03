using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.SeedWork.SearchableRepository;
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
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 15;
    private const int MaxPageNumber = 1000;


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

    /// <summary>
    /// Cancels an existing sale.
    /// </summary>
    /// <param name="id">The sale ID to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cancelled sale details.</returns>
    [HttpPut("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSale(Guid id, CancellationToken cancellationToken)
    {
        var command = new CancelSaleCommand { SaleId = id };
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<CancelSaleResult>
        {
            Success = true,
            Message = "Sale cancelled successfully.",
            Data = result
        });
    }

    /// <summary>
    /// Cancels a specific item in a sale.
    /// </summary>
    /// <param name="id">The sale ID.</param>
    /// <param name="itemId">The item ID to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cancelled item details.</returns>
    [HttpPut("{id:guid}/items/{itemId:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelItemResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelItem(Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var command = new CancelItemCommand { SaleId = id, ItemId = itemId };
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<CancelItemResult>
        {
            Success = true,
            Message = "Item cancelled successfully.",
            Data = result
        });
    }


    /// <summary>
    /// List sales with pagination, search and ordering capabilities
    /// </summary>
    /// <param name="page">Page number (starting at 1)</param>
    /// <param name="perPage">Items per page (max 100)</param>
    /// <param name="search">Optional search term</param>
    /// <param name="sort">Optional field name for sorting</param>
    /// <param name="dir">Sort direction (Asc or Desc)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of sales</returns>
    /// <response code="200">Returns the paginated list of sales</response>
    /// <response code="400">If the request parameters are invalid</response>
    [HttpGet]
    [ProducesResponseType(typeof(ListSalesOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
        [FromQuery] int? page = null,
        [FromQuery] int? perPage = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = page ?? 1;
        var pageSize = perPage ?? DefaultPageSize;

        // Validação do número da página
        if (pageNumber <= 0)
        {
            return BadRequest(new
            {
                error = "Page number must be greater than 0"
            });
        }

        if (pageNumber > MaxPageNumber)
        {
            return BadRequest(new
            {
                error = $"Page number cannot be greater than {MaxPageNumber}"
            });
        }

        // Validação do tamanho da página
        if (pageSize <= 0)
        {
            return BadRequest(new
            {
                error = "Page size must be greater than 0"
            });
        }

        if (pageSize > MaxPageSize)
        {
            return BadRequest(new
            {
                error = $"Page size cannot be greater than {MaxPageSize}"
            });
        }

        // Validação do parâmetro de busca
        if (!string.IsNullOrEmpty(search) && search.Length > 100)
        {
            return BadRequest(new
            {
                error = "Search term cannot be longer than 100 characters"
            });
        }

        // Validação do parâmetro de ordenação
        if (!string.IsNullOrEmpty(sort) && sort.Length > 50)
        {
            return BadRequest(new
            {
                error = "Sort parameter cannot be longer than 50 characters"
            });
        }

        var command = new ListSalesCommand(
            pageNumber,
            pageSize,
            search,
            sort,
            dir ?? SearchOrder.Asc
        );

        var output = await _mediator.Send(command, cancellationToken);
        return Ok(output);
    }
}