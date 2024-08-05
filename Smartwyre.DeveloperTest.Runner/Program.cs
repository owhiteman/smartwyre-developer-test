using System;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var services = ConfigureServices();

        var serviceProvider = services.BuildServiceProvider();

        var rebateService = serviceProvider.GetService<IRebateService>();

        var request = new CalculateRebateRequest();
        var result = rebateService.Calculate(request);
        Console.WriteLine("Rebate Calculation Completed. " + result.Success);

    }

    private static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddTransient<IProductDataStore, ProductDataStore>();
        services.AddTransient<IRebateDataStore, RebateDataStore>();
        services.AddTransient<IRebateService, RebateService>();

        services.AddTransient<FixedCashAmountCalculator>();
        services.AddTransient<FixedRateRebateCalculator>();
        services.AddTransient<AmountPerUomCalculator>();

        services.AddTransient<IRebateCalculator, FixedCashAmountCalculator>(provider => provider.GetRequiredService<FixedCashAmountCalculator>());
        services.AddTransient<IRebateCalculator, FixedRateRebateCalculator>(provider => provider.GetRequiredService<FixedRateRebateCalculator>());
        services.AddTransient<IRebateCalculator, AmountPerUomCalculator>(provider => provider.GetRequiredService<AmountPerUomCalculator>());

        services.AddSingleton<IRebateCalculatorFactory, RebateCalculatorFactory>();

        return services;
    }
}
