using Microsoft.Extensions.Configuration;
using TaxCalculator.Infrastructure.Interfaces.Configuration;

namespace TaxCalculator.Infrastructure.Configuration
{
    public class TaxConfigurationProvider : ITaxConfigurationProvider
    {
        private readonly TaxCalculatorConfiguration _taxConfiguration;

        public TaxConfigurationProvider(IConfiguration configuration)
        {
            _taxConfiguration = configuration.GetSection("TaxCalculaterConfiguration").Get<TaxCalculatorConfiguration>();
        }

        public TaxCalculatorConfiguration GetConfiguration()
        {
            return _taxConfiguration;
        }
    }
}
