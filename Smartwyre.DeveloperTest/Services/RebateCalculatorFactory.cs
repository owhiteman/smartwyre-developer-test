using System;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Types;

public class RebateCalculatorFactory : IRebateCalculatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public RebateCalculatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IRebateCalculator GetCalculator(IncentiveType incentiveType)
    {
        switch (incentiveType)
        {
            case IncentiveType.FixedCashAmount:
                return _serviceProvider.GetRequiredService<FixedCashAmountCalculator>();
            case IncentiveType.FixedRateRebate:
                return _serviceProvider.GetRequiredService<FixedRateRebateCalculator>();
            case IncentiveType.AmountPerUom:
                return _serviceProvider.GetRequiredService<AmountPerUomCalculator>();
            default:
                throw new ArgumentException("Invalid incentive type");
        }
    }
}
