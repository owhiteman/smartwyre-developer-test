using Smartwyre.DeveloperTest.Types;

public class FixedRateRebateCalculator : IRebateCalculator
{
    public decimal CalculateResult(CalculateRebateResult result, Product product, Rebate rebate, decimal volume)
    {
        var rebateAmount = 0m;
        if (product == null)
        {
            result.Success = false;
        }
        else if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
        {
            result.Success = false;
        }
        else if (rebate.Percentage == 0 || product.Price == 0 || volume == 0)
        {
            result.Success = false;
        }
        else
        {
            rebateAmount += product.Price * rebate.Percentage * volume;
            result.Success = true;
        }

        return rebateAmount;
    }
}