using MediatR;
using Ambev.DeveloperEvaluation.Domain.SeedWork.SearchableRepository;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesCommand : IRequest<ListSalesOutput>
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string? Search { get; set; }
    public string? OrderBy { get; set; }
    public SearchOrder Order { get; set; }

    public ListSalesCommand(
        int page = 1,
        int perPage = 15,
        string? search = null,
        string? orderBy = null,
        SearchOrder order = SearchOrder.Asc)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        OrderBy = orderBy;
        Order = order;
    }
}