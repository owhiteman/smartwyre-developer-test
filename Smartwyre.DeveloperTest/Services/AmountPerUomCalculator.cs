using Smartwyre.DeveloperTest.Types;

public class AmountPerUomCalculator : IRebateCalculator
{
    public decimal CalculateResult(CalculateRebateResult result, Product product, Rebate rebate, decimal volume)
    {
        var rebateAmount = 0m;

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
        {
            result.Success = false;
        }
        else if (rebate.Amount == 0 || volume == 0)
        {
            result.Success = false;
        }
        else
        {
            rebateAmount += rebate.Amount * volume;
            result.Success = true;
        }

        return rebateAmount;
    }
}