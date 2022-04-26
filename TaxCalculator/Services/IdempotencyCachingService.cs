using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TaxCalculator.Infrastructure.Exceptions;
using TaxCalculator.Infrastructure.Helpers;
using TaxCalculator.Infrastructure.Interfaces.Models.Entities;
using TaxCalculator.Infrastructure.Interfaces.Services;
using TaxCalculator.Infrastructure.Resources;
using TaxCalculator.Models.Entities;

namespace TaxCalculator.Services
{
    /// <summary>
    /// A caching service class with implementation of a naive approach to idempotency (without client generated request tokens)
    /// </summary>
    public class IdempotencyCachingService : IIdempotencyService
    {
        private const string _cacheKey = "requests";

        private readonly IMemoryCache _memoryCache;
        private readonly HashAlgorithm _hashAlgorithm;

        public IdempotencyCachingService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _hashAlgorithm = new SHA256CryptoServiceProvider();
        }

       
        /// <inheritdoc/>
        /// <remarks>Only request with <see cref="HttpMethods.Post"/> and <see cref="HttpMethods.Patch"/> are checked for uniqueness.</remarks>
        public async Task<bool> IsUniqueRequest(HttpRequest request)
        {
            if (request.Method != HttpMethods.Post
                && request.Method != HttpMethods.Patch)
            {
                return true;
            }

            if (!_memoryCache.TryGetValue(_cacheKey, out Dictionary<string, string> requests))
            {
                _memoryCache.Set(_cacheKey, new Dictionary<string, string>());
                return true;
            }

            string requestHash = await IdempotencyHelper.GetRequestsDataHash(request, _hashAlgorithm);
            if (requests.ContainsKey(requestHash))
            {
                return false;
            }

            return true;
        }
        
        /// <inheritdoc />
        /// <remarks>Uses <see cref="IMemoryCache"/> as a response storage</remarks>
        public async Task<IIncomeTaxPayer> GetIncomeTaxResponse(HttpRequest request)
        {
            IncomeTaxPayer response = null;
            
            if (!_memoryCache.TryGetValue(_cacheKey, out Dictionary<string, string> requests))
            {
                _memoryCache.Set(_cacheKey, new Dictionary<string, string>());
                return response;
            }

            string requestHash = await IdempotencyHelper.GetRequestsDataHash(request, _hashAlgorithm);
            if (!requests.ContainsKey(requestHash))
            {
                return response;
            }

            string responseJson = requests[requestHash];
            response = IdempotencyHelper.DeSerialize<IncomeTaxPayer>(responseJson);

            return response;
        }

        /// <inheritdoc />
        /// <remarks>Uses <see cref="IMemoryCache"/> as a data storage</remarks>
        /// <exception cref="DuplicateRequestKeyException">When the request is not unique or has already been made.</exception>
        public async Task AddIncomeTaxResponse(HttpRequest request, IIncomeTaxPayer response)
        { 
            string jsonResponse = IdempotencyHelper.Serialize(response);
            string requestHash = await IdempotencyHelper.GetRequestsDataHash(request, _hashAlgorithm);

            if (!_memoryCache.TryGetValue(_cacheKey, out Dictionary<string, string> requests))
            {
                Dictionary<string, string> requestResponseDict = new Dictionary<string, string>();
                requestResponseDict.Add(requestHash, jsonResponse);
                _memoryCache.Set(_cacheKey, requestResponseDict);
                return;
            }

            if (requests.ContainsKey(requestHash))
            {
                throw new DuplicateRequestKeyException(ExceptionMessages.SameRequestAlreadyInCache);
            }

            requests.Add(requestHash, jsonResponse);
        }
    }
}
