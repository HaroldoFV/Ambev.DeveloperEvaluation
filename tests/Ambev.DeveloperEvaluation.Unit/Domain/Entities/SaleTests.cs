using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover item management, cancellation, and total value calculation.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that the Sale constructor initializes properties correctly.
    /// </summary>
    [Fact(DisplayName = "Constructor should initialize properties correctly")]
    public void Given_ValidParameters_When_Constructed_Then_PropertiesShouldBeInitialized()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        // Act
        var sale = new Sale(customerId, branchId);

        // Assert
        Assert.Equal(customerId, sale.CustomerId);
        Assert.Equal(branchId, sale.BranchId);
        Assert.Empty(sale.Items);
        Assert.False(sale.IsCancelled);
        Assert.Equal(0.0m, sale.TotalValue);
    }

    /// <summary>
    /// Tests that adding an item updates the total value and item list.
    /// </summary>
    [Fact(DisplayName = "Adding an item should update total value and item list")]
    public void Given_ValidItem_When_Added_Then_TotalValueAndItemsShouldBeUpdated()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var saleItem = new SaleItem(Guid.NewGuid(), 5, 10.0m);

        // Act
        sale.AddItem(saleItem);

        // Assert
        Assert.Single(sale.Items);
        Assert.Equal(saleItem, sale.Items.First());
        Assert.Equal(45.0m, sale.TotalValue);
    }

    /// <summary>
    /// Tests that adding an item with a quantity exceeding the limit throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding an item with quantity exceeding limit should throw exception")]
    public void Given_ItemWithExcessiveQuantity_When_Added_Then_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var saleItem = new SaleItem(Guid.NewGuid(), 25, 10.0m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sale.AddItem(saleItem));
    }

    /// <summary>
    /// Tests that removing an item updates the total value and item list.
    /// </summary>
    [Fact(DisplayName = "Removing an item should update total value and item list")]
    public void Given_ExistingItem_When_Removed_Then_TotalValueAndItemsShouldBeUpdated()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var saleItem = new SaleItem(Guid.NewGuid(), 5, 10.0m);
        sale.AddItem(saleItem);

        // Act
        sale.RemoveItem(saleItem);

        // Assert
        Assert.Empty(sale.Items);
        Assert.Equal(0.0m, sale.TotalValue);
    }

    /// <summary>
    /// Tests that cancelling a sale sets its status to cancelled.
    /// </summary>
    [Fact(DisplayName = "Cancelling a sale should set its status to cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.IsCancelled);
    }

    /// <summary>
    /// Tests that the total value is calculated correctly for multiple items.
    /// </summary>
    [Fact(DisplayName = "Total value should be calculated correctly for multiple items")]
    public void Given_MultipleItems_When_Added_Then_TotalValueShouldBeCorrect()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.AddItem(new SaleItem(Guid.NewGuid(), 5, 100));
        sale.AddItem(new SaleItem(Guid.NewGuid(), 10, 50));

        // Assert
        Assert.Equal(850, sale.TotalValue);
    }

    /// <summary>
    /// Tests that adding units to an existing item updates its quantity and recalculates the total value.
    /// </summary>
    [Fact(DisplayName = "Adding units to an existing item should update quantity and total value")]
    public void Given_ExistingItem_When_UnitsAdded_Then_QuantityAndTotalValueShouldBeUpdated()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var productId = Guid.NewGuid();
        var existingItem = new SaleItem(productId, 5, 20.0m);
        sale.AddItem(existingItem);

        // Act
        var additionalItem = new SaleItem(productId, 10, 20.0m);
        sale.AddItem(additionalItem);

        // Assert
        Assert.Single(sale.Items);
        var updatedItem = sale.Items.First();
        Assert.Equal(15, updatedItem.Quantity); // 5 + 10
        Assert.Equal(240.0m, sale.TotalValue); // 15 * 20 with 20% discount
    }
}