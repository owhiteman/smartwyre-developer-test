using Xunit;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class AmountPerUomCalculatorTests
{
    [Fact]
    public void CalculateResult_WithNullProduct_ReturnsFailure()
    {
        // Arrange
        var calculator = new AmountPerUomCalculator();
        var result = new CalculateRebateResult();
        var rebate = new Rebate { Amount = 100m };

        // Act
        var rebateAmount = calculator.CalculateResult(result, null, rebate, 10);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, rebateAmount);
    }

    [Fact]
    public void CalculateResult_WithUnsupportedIncentive_ReturnsFailure()
    {
        // Arrange
        var calculator = new AmountPerUomCalculator();
        var result = new CalculateRebateResult();
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var rebate = new Rebate { Amount = 100m };

        // Act
        var rebateAmount = calculator.CalculateResult(result, product, rebate, 10);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, rebateAmount);
    }

    [Fact]
    public void CalculateResult_WithZeroRebateAmountOrVolume_ReturnsFailure()
    {
        // Arrange
        var calculator = new AmountPerUomCalculator();
        var result = new CalculateRebateResult();
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var rebate = new Rebate { Amount = 0m };

        // Act
        var rebateAmount = calculator.CalculateResult(result, product, rebate, 10);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, rebateAmount);
    }

    [Fact]
    public void CalculateResult_ValidInputs_ReturnsSuccessAndCorrectAmount()
    {
        // Arrange
        var calculator = new AmountPerUomCalculator();
        var result = new CalculateRebateResult();
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var rebate = new Rebate { Amount = 100m };

        // Act
        var rebateAmount = calculator.CalculateResult(result, product, rebate, 10);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1000m, rebateAmount);
    }
}
