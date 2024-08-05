using System;
using Smartwyre.DeveloperTest.Types;

public class FixedCashAmountCalculator : IRebateCalculator
{
    public decimal CalculateResult(CalculateRebateResult result, Product product, Rebate rebate, decimal volume)
    {
        var rebateAmount = 0m;
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
        {
            result.Success = false;
        }
        else if (rebate.Amount == 0)
        {
            result.Success = false;
        }
        else
        {
            rebateAmount = rebate.Amount;
            result.Success = true;
        }
        return rebateAmount;
    }

}