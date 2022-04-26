using TaxCalculator.Infrastructure.Configuration;

namespace TaxCalculator.Infrastructure.Interfaces.Configuration
{
    public interface ITaxConfigurationProvider
    {
        TaxCalculatorConfiguration GetConfiguration();
    }
}
