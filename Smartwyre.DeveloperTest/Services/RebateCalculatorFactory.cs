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
        return _serviceProvider.GetRequiredKeyedService<IRebateCalculator>(incentiveType);
    }
}
