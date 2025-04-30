using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover property initialization and discount application.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that the SaleItem constructor initializes properties correctly.
    /// </summary>
    [Fact(DisplayName = "Constructor should initialize properties correctly")]
    public void Given_ValidParameters_When_Constructed_Then_PropertiesShouldBeInitialized()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = 5;
        var unitPrice = 10.0m;

        // Act
        var saleItem = new SaleItem(productId, quantity, unitPrice);

        // Assert
        Assert.Equal(productId, saleItem.ProductId);
        Assert.Equal(quantity, saleItem.Quantity);
        Assert.Equal(unitPrice, saleItem.UnitPrice);
        Assert.Equal(0.10m, saleItem.Discount);
        Assert.Equal(45.0m, saleItem.TotalValue);
    }

    /// <summary>
    /// Tests that the constructor throws an exception for invalid quantities.
    /// </summary>
    [Fact(DisplayName = "Constructor should throw exception for invalid quantities")]
    public void Given_InvalidQuantity_When_Constructed_Then_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var unitPrice = 10.0m;

        // Act & Assert
        Assert.Throws<EntityValidationException>(() => new SaleItem(productId, 0, unitPrice));
        Assert.Throws<EntityValidationException>(() => new SaleItem(productId, -1, unitPrice));
    }

    /// <summary>
    /// Tests that a 10% discount is applied for quantities between 4 and 9.
    /// </summary>
    [Fact(DisplayName = "10% discount should be applied for quantities between 4 and 9")]
    public void Given_QuantityBetween4And9_When_Calculated_Then_ShouldApply10PercentDiscount()
    {
        // Act
        var item = new SaleItem(Guid.NewGuid(), 5, 100);

        // Assert
        Assert.Equal(450, item.TotalValue);
    }

    /// <summary>
    /// Tests that a 20% discount is applied for quantities between 10 and 20.
    /// </summary>
    [Fact(DisplayName = "20% discount should be applied for quantities between 10 and 20")]
    public void Given_QuantityBetween10And20_When_Calculated_Then_ShouldApply20PercentDiscount()
    {
        // Act
        var item = new SaleItem(Guid.NewGuid(), 15, 100);

        // Assert
        Assert.Equal(1200, item.TotalValue);
    }

    /// <summary>
    /// Tests that no discount is applied for quantities less than 4.
    /// </summary>
    [Fact(DisplayName = "No discount should be applied for quantities less than 4")]
    public void Given_QuantityLessThan4_When_Calculated_Then_ShouldNotApplyDiscount()
    {
        // Act
        var item = new SaleItem(Guid.NewGuid(), 3, 100);

        // Assert
        Assert.Equal(300, item.TotalValue);
    }

    /// <summary>
    /// Tests that AssociateItem sets the SaleId correctly.
    /// </summary>
    [Fact(DisplayName = "AssociateItem should set SaleId correctly")]
    public void Given_ValidSaleId_When_AssociateItemCalled_Then_SaleIdShouldBeSet()
    {
        // Arrange
        var saleItem = new SaleItem(Guid.NewGuid(), 5, 100);
        var saleId = Guid.NewGuid();

        // Act
        saleItem.AssociateItem(saleId);

        // Assert
        Assert.Equal(saleId, saleItem.SaleId);
    }

    /// <summary>
    /// Tests that AssociateItem throws an exception for an empty SaleId.
    /// </summary>
    [Fact(DisplayName = "AssociateItem should throw exception for empty SaleId")]
    public void Given_EmptySaleId_When_AssociateItemCalled_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = new SaleItem(Guid.NewGuid(), 5, 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => saleItem.AssociateItem(Guid.Empty));
    }
}