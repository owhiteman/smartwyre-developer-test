using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IProductDataStore _productDataStore;
    private readonly IRebateDataStore _rebateDataStore;
private readonly IRebateCalculatorFactory _calculatorFactory;

    public RebateService(
        IProductDataStore productStore, 
        IRebateDataStore rebateDataStore, 
        IRebateCalculatorFactory calculatorFactory)
        {       
        _productDataStore = productStore;
        _rebateDataStore = rebateDataStore;
        _calculatorFactory = calculatorFactory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {

        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        if (rebate == null)
        {
            result.Success = false;
            return result;
        }

        var calculator = _calculatorFactory.GetCalculator(rebate.Incentive);
        var rebateAmount = calculator.CalculateResult(result, product, rebate, request.Volume);

        if (result.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
        }

        return result;
    }
}
