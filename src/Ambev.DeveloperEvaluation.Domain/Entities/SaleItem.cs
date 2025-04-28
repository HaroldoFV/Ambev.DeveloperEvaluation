using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalValue { get; private set; }

    public SaleItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        ApplyDiscount();
        CalculateTotal();
    }

    public void AssociateItem(Guid saleId)
    {
        if (saleId == Guid.Empty)
            throw new ArgumentException("SaleId must be a valid GUID.", nameof(saleId));

        SaleId = saleId;
    }

    private void ApplyDiscount()
    {
        if (Quantity >= 10 && Quantity <= 20)
            Discount = 0.20m;
        else if (Quantity >= 4)
            Discount = 0.10m;
        else
            Discount = 0.0m;
    }

    private void CalculateTotal()
    {
        TotalValue = Quantity * UnitPrice * (1 - Discount);
    }

    internal void AddUnits(int units)
    {
        if (units <= 0)
            throw new ArgumentException("Units must be greater than zero.", nameof(units));

        Quantity += units;
        ApplyDiscount();
        CalculateTotal();
    }
}