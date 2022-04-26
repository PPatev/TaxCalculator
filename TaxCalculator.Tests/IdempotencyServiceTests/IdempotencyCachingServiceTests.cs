using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Infrastructure.Helpers;
using TaxCalculator.Infrastructure.Interfaces.Models.Entities;
using TaxCalculator.Models.Dtos;
using TaxCalculator.Models.Entities;
using TaxCalculator.Services;

namespace TaxCalculator.UnitTests.IdempotencyServiceTests
{
    
    public class IdempotencyCachingServiceTests
    {
        private string _requestBodyJson;
        private IIncomeTaxPayer _response;

        [SetUp]
        public void SetUp()
        {
            _requestBodyJson = JsonConvert.SerializeObject(new TaxPayerInputModel { });
            _response = new IncomeTaxPayer();
        }

        [Test]
        [TestCase("GET")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        public async Task IsUniqueRequest_WhenCalledWithWrongRequestMethod_ShouldReturnTrue(string method)
        {
            using (IMemoryCache cache = new MemoryCache(new MemoryCacheOptions()))
            {
                IdempotencyCachingService service = new IdempotencyCachingService(cache);
                DefaultHttpContext httpContext = new DefaultHttpContext();
                httpContext.Request.Method = method;

                bool result = await service.IsUniqueRequest(httpContext.Request);

                Assert.IsTrue(result);
            }
        }

        [Test]
        public async Task IsUniqueRequest_WhenCalledWithPostRequest_ShouldReturnTrue()
        {
            using (IMemoryCache cache = new MemoryCache(new MemoryCacheOptions()))
            {
                IdempotencyCachingService service = new IdempotencyCachingService(cache);
                DefaultHttpContext httpContext = new DefaultHttpContext();
                httpContext.Request.Method = "POST";

                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(_requestBodyJson)))
                {
                    httpContext.Request.Body = stream;
                    httpContext.Request.ContentLength = stream.Length;

                    bool result = await service.IsUniqueRequest(httpContext.Request);
                    Assert.IsTrue(result);
                }
            }
        }

        [Test]
        public async Task IsUniqueRequest_WhenCalledWithSameRequest_ShouldReturnFalse()
        {
            using (IMemoryCache cache = new MemoryCache(new MemoryCacheOptions()))
            {
                IdempotencyCachingService service = new IdempotencyCachingService(cache);
                DefaultHttpContext httpContext = new DefaultHttpContext();
                httpContext.Request.Method = "POST";

                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(_requestBodyJson)))
                {
                    httpContext.Request.Body = stream;
                    httpContext.Request.ContentLength = stream.Length;

                    HashAlgorithm algorithm = new SHA256CryptoServiceProvider();
                    string hash = await IdempotencyHelper.GetRequestsDataHash(httpContext.Request, algorithm);
                    Dictionary<string, string> requestResponseDict = new Dictionary<string, string>();
                    requestResponseDict.Add(hash, _requestBodyJson);

                    cache.Set("requests", requestResponseDict);
                    
                    bool result = await service.IsUniqueRequest(httpContext.Request);

                    Assert.IsFalse(result);
                }
            }
        }

        [Test]
        public async Task AddIncomeTaxResponse_WhenCalled_ShouldAddInCache()
        {
            using (IMemoryCache cache = new MemoryCache(new MemoryCacheOptions()))
            {
                IdempotencyCachingService service = new IdempotencyCachingService(cache);
                DefaultHttpContext httpContext = new DefaultHttpContext();
                httpContext.Request.Method = "POST";

                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(_requestBodyJson)))
                {
                    httpContext.Request.Body = stream;
                    httpContext.Request.ContentLength = stream.Length;

                    await service.AddIncomeTaxResponse(httpContext.Request, _response);

                    cache.TryGetValue("requests", out Dictionary<string, string> requestResponseDict);

                    Assert.That(requestResponseDict.Count, Is.EqualTo(1));
                }
            }
        }

        [Test]
        public async Task AddIncomeTaxResponse_WhenCalled_ShouldAddCorrectObject()
        {
            using (IMemoryCache cache = new MemoryCache(new MemoryCacheOptions()))
            {
                IdempotencyCachingService service = new IdempotencyCachingService(cache);
                DefaultHttpContext httpContext = new DefaultHttpContext();
                httpContext.Request.Method = "POST";

                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(_requestBodyJson)))
                {
                    httpContext.Request.Body = stream;
                    httpContext.Request.ContentLength = stream.Length;

                    await service.AddIncomeTaxResponse(httpContext.Request, _response);

                    cache.TryGetValue("requests", out Dictionary<string, string> requestResponseDict);
                    string cachedResponse = requestResponseDict.First().Value;
                    string jsonResponse = JsonConvert.SerializeObject(_response);

                    Assert.That(jsonResponse, Is.EqualTo(cachedResponse).NoClip);
                }
            }
        }

        [Test]
        public async Task GetIncomeTaxResponse_WhenCalled_ShouldReturnCorrectObject()
        {
            using (IMemoryCache cache = new MemoryCache(new MemoryCacheOptions()))
            {
                IdempotencyCachingService service = new IdempotencyCachingService(cache);
                DefaultHttpContext httpContext = new DefaultHttpContext();
                httpContext.Request.Method = "POST";

                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(_requestBodyJson)))
                {
                    httpContext.Request.Body = stream;
                    httpContext.Request.ContentLength = stream.Length;

                    await service.AddIncomeTaxResponse(httpContext.Request, _response);

                    IIncomeTaxPayer actualResponseObject = await service.GetIncomeTaxResponse(httpContext.Request);

                    Assert.IsTrue(_response.Equals(actualResponseObject));
                }
            }
        }
    }
}
