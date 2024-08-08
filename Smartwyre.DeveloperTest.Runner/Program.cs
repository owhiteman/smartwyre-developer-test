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

        Console.WriteLine("Enter Rebate Identifier:");
        request.RebateIdentifier = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(request.RebateIdentifier))
        {
            Console.WriteLine("Rebate Identifier cannot be empty. Please enter a valid identifier:");
            request.RebateIdentifier = Console.ReadLine();
        }

        Console.WriteLine("Enter Product Identifier:");
        request.ProductIdentifier = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(request.ProductIdentifier))
        {
            Console.WriteLine("Product Identifier cannot be empty. Please enter a valid identifier:");
            request.ProductIdentifier = Console.ReadLine();
        }

        Console.WriteLine("Enter Volume:");
        string volumeInput = Console.ReadLine();
        decimal volume;
        while (!decimal.TryParse(volumeInput, out volume))
        {
            Console.WriteLine("Invalid input for Volume. Please enter a valid decimal number:");
            volumeInput = Console.ReadLine();
        }
        request.Volume = volume;

        var result = rebateService.Calculate(request);
        Console.WriteLine("Rebate Calculation Completed. " + result.Success);

    }

    private static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddTransient<IProductDataStore, ProductDataStore>();
        services.AddTransient<IRebateDataStore, RebateDataStore>();
        services.AddTransient<IRebateService, RebateService>();

        services.AddKeyedTransient<IRebateCalculator, FixedCashAmountCalculator>(IncentiveType.FixedCashAmount);
        services.AddKeyedTransient<IRebateCalculator, FixedRateRebateCalculator>(IncentiveType.FixedRateRebate);
        services.AddKeyedTransient<IRebateCalculator, AmountPerUomCalculator>(IncentiveType.AmountPerUom);

        services.AddSingleton<IRebateCalculatorFactory, RebateCalculatorFactory>();

        return services;
    }
}
