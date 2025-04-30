using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.ToString());

        var saleItems = command.Items.Select(item => new SaleItem
        (
            item.ProductId,
            item.Quantity,
            item.Price
        )).ToList();

        var sale = new Sale(command.CustomerId, command.BranchId);
        saleItems.ForEach(i => sale.AddItem(i));

        await _saleRepository.CreateAsync(sale, cancellationToken);
        await _saleRepository.UnitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}