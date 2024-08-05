using Smartwyre.DeveloperTest.Types;

public interface IRebateDataStore
{
    Rebate GetRebate(string rebateIdentifier);

    void StoreCalculationResult(Rebate account, decimal rebateAmount);
}