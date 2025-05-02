using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.ToString());

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);

        sale.UpdateCustomer(command.CustomerId);
        sale.UpdateBranch(command.BranchId);

        foreach (var itemDto in command.Items)
        {
            var saleItem = new SaleItem(itemDto.ProductId, itemDto.Quantity, itemDto.UnitPrice);
            sale.AddOrUpdateItem(saleItem);
        }

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        sale.RaiseEvent(new SaleModifiedEvent(sale));

        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<UpdateSaleResult>(sale);
    }
}