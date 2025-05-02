using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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

        sale.RaiseEvent(new SaleCreatedEvent(sale));

        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}