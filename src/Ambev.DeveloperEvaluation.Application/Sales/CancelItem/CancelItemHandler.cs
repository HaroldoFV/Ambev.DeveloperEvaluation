using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

public class CancelItemHandler : IRequestHandler<CancelItemCommand, CancelItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CancelItemHandler(ISaleRepository saleRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CancelItemResult> Handle(CancelItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.ToString());

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        var item = sale.Items.FirstOrDefault(i => i.Id == command.ItemId);

        if (item == null)
            throw new NotFoundException($"Item with id {command.ItemId} not found in sale {command.SaleId}");

        try
        {
            if (item.IsCancelled)
                throw new EntityValidationException("Item is already cancelled");

            item.Cancel();
            await _saleRepository.UpdateAsync(sale, cancellationToken);

            sale.RaiseEvent(new ItemCancelledEvent(sale, item));

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<CancelItemResult>((sale, command.ItemId));
        }
        catch (EntityValidationException)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}