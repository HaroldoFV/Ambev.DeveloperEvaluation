using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CancelSaleHandler(ISaleRepository saleRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.ToString());

        try
        {
            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);

            sale.Cancel();

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            sale.RaiseEvent(new SaleCancelledEvent(sale));

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<CancelSaleResult>(sale);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}