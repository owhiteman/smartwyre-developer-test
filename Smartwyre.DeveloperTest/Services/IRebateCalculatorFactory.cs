using Smartwyre.DeveloperTest.Types;

public interface IRebateCalculatorFactory
{
    IRebateCalculator GetCalculator(IncentiveType incentiveType);
}
