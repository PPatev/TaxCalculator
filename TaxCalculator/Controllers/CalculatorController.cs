using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TaxCalculator.Infrastructure.Filters;
using TaxCalculator.Infrastructure.Interfaces.Models.Entities;
using TaxCalculator.Infrastructure.Interfaces.Services;
using TaxCalculator.Models.Dtos;

namespace TaxCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly IIdempotencyService _idempotencyService;
        private readonly IIncomeTaxService _incomeTaxService;
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(IIdempotencyService idempotencyService, IIncomeTaxService incomeTaxService, ILogger<CalculatorController> logger)
        {
            _idempotencyService = idempotencyService;
            _incomeTaxService = incomeTaxService;
            _logger = logger;
        }

        [HttpPost]
        [RequestBodyReadable]
        [ProducesResponseType(typeof(IIncomeTaxPayer), 200)]
        public async Task<ActionResult<IIncomeTaxPayer>> Calculate([FromBody]TaxPayerInputModel taxPayerContract)
        {
            bool isUniqueRequest = await _idempotencyService.IsUniqueRequest(HttpContext.Request);
            if (!isUniqueRequest)
            {
                var response = await _idempotencyService.GetIncomeTaxResponse(HttpContext.Request);
                return new ActionResult<IIncomeTaxPayer>(response);
            }

            IIncomeTaxPayer result = _incomeTaxService.CalculatePersonalTaxes(taxPayerContract);
            await _idempotencyService.AddIncomeTaxResponse(HttpContext.Request, result);

            return new ActionResult<IIncomeTaxPayer>(result);
        }
    }
}
