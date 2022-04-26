using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TaxCalculator.Infrastructure.Interfaces.Models.Entities;

namespace TaxCalculator.Infrastructure.Interfaces.Services
{
    public interface IIdempotencyService
    {
        /// <summary>
        /// Checks whether the provided <see cref="HttpRequest"/> has already been made.
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/></param>
        /// <returns><see cref="Boolean"/> value. True if it is a new and unique request.</returns>
        Task<bool> IsUniqueRequest(HttpRequest request);

        /// <summary>
        /// Gets the response for the given request.
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/></param>
        /// <returns><see cref="IIncomeTaxPayer"/>. <see langword="null"/> if there is no corresponding response.</returns>
        Task<IIncomeTaxPayer> GetIncomeTaxResponse(HttpRequest request);

        /// <summary>
        /// Adds a <see cref="HttpRequest"/> and a <see cref="IIncomeTaxPayer"/> response to a data storage
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/></param>
        /// <param name="response"><see cref="IIncomeTaxPayer"/></param>
        Task AddIncomeTaxResponse(HttpRequest request, IIncomeTaxPayer response);
    }
}
