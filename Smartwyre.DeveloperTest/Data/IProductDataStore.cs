using Smartwyre.DeveloperTest.Types;

public interface IProductDataStore
{
    Product GetProduct(string productIdentifier);
}