namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesOutput
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public List<ListSalesResult> Items { get; set; }

    public ListSalesOutput(
        int currentPage,
        int perPage,
        int total,
        List<ListSalesResult> items)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}