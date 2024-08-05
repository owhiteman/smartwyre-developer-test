using System;
using Smartwyre.DeveloperTest.Types;

public interface IRebateCalculator{
    decimal CalculateResult(CalculateRebateResult result, Product product, Rebate rebate, decimal volume);
}