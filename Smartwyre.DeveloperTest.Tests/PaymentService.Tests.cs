using Moq;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private Mock<IProductDataStore> _mockProductStore;
    private Mock<IRebateDataStore> _mockRebateStore;
    private Mock<IRebateCalculatorFactory> _mockCalculatorFactory;
    private readonly Mock<IRebateCalculator> _mockCalculator;
    private RebateService _rebateService;

    public PaymentServiceTests()
    {
        _mockProductStore = new Mock<IProductDataStore>();
        _mockRebateStore = new Mock<IRebateDataStore>();
        _mockCalculatorFactory = new Mock<IRebateCalculatorFactory>();
        _mockCalculator = new Mock<IRebateCalculator>();
        _rebateService = new RebateService(_mockProductStore.Object, _mockRebateStore.Object, _mockCalculatorFactory.Object);
    }

    [Fact]
    public void Calculate_WhenRebateIsNull_ReturnsFailure()
    {
        // Arrange
        var request = new CalculateRebateRequest { ProductIdentifier = "123", RebateIdentifier = "456" };
        _mockRebateStore.Setup(x => x.GetRebate("456")).Returns((Rebate)null);
        _mockProductStore.Setup(x => x.GetProduct("123")).Returns((Product)null);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WhenCalculatorModifiesResult_SuccessIsTrue()
    {
        // Arrange
        var request = new CalculateRebateRequest { ProductIdentifier = "123", RebateIdentifier = "456", Volume = 10 };
        var rebate = new Rebate { Identifier = "456", Incentive = IncentiveType.FixedRateRebate };
        var product = new Product { Identifier = "123", Price = 100 };

        _mockRebateStore.Setup(x => x.GetRebate("456")).Returns(rebate);
        _mockProductStore.Setup(x => x.GetProduct("123")).Returns(product);
        _mockCalculatorFactory.Setup(x => x.GetCalculator(rebate.Incentive)).Returns(_mockCalculator.Object);

        _mockCalculator.Setup(x => x.CalculateResult(It.IsAny<CalculateRebateResult>(), product, rebate, request.Volume))
                       .Returns(100m)
                       .Callback<CalculateRebateResult, Product, Rebate, decimal>((result, prod, reb, vol) => result.Success = true);  // Simulating the modification

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        _mockRebateStore.Verify(x => x.StoreCalculationResult(rebate, 100m), Times.Once);
    }

    [Fact]
    public void Calculate_WhenCalculatorModifiesResult_SuccessIsFalse()
    {
        // Arrange
        var request = new CalculateRebateRequest { ProductIdentifier = "123", RebateIdentifier = "456", Volume = 10 };
        var rebate = new Rebate { Identifier = "456", Incentive = IncentiveType.FixedRateRebate };
        var product = new Product { Identifier = "123", Price = 100 };

        _mockRebateStore.Setup(x => x.GetRebate("456")).Returns(rebate);
        _mockProductStore.Setup(x => x.GetProduct("123")).Returns(product);
        _mockCalculatorFactory.Setup(x => x.GetCalculator(rebate.Incentive)).Returns(_mockCalculator.Object);

        _mockCalculator.Setup(x => x.CalculateResult(It.IsAny<CalculateRebateResult>(), product, rebate, request.Volume))
                       .Returns(100m)
                       .Callback<CalculateRebateResult, Product, Rebate, decimal>((result, prod, reb, vol) => result.Success = false);  // Simulating the modification

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }
}
