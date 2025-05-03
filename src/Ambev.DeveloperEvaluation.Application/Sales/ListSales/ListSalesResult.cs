namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesResult
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; private set; }
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; }
    public List<SaleItemResult> Items { get; set; }

    public ListSalesResult()
    {
        Items = new List<SaleItemResult>();
    }

    public ListSalesResult(
        Guid id,
        Guid customerId,
        decimal totalAmount,
        DateTime saleDate,
        List<SaleItemResult> items, Guid branchId)
    {
        Id = id;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        SaleDate = saleDate;
        Items = items;
        BranchId = branchId;
    }
}

public class SaleItemResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }

    public SaleItemResult()
    {
    }

    public SaleItemResult(
        Guid productId,
        int quantity,
        decimal unitPrice,
        decimal subTotal)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        SubTotal = subTotal;
    }
}