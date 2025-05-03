using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.SeedWork.SearchableRepository;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesOutput>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<ListSalesOutput> Handle(ListSalesCommand request,
        CancellationToken cancellationToken)
    {
        var searchInput = new SearchInput(
            request.Page,
            request.PerPage,
            request.Search,
            request.OrderBy,
            request.Order
        );

        var searchOutput = await _saleRepository.Search(searchInput, cancellationToken);
        var items = _mapper.Map<List<ListSalesResult>>(searchOutput.Items);

        return new ListSalesOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            items
        );
    }
}