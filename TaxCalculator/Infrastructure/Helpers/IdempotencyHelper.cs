using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Infrastructure.Helpers
{
    public static class IdempotencyHelper
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettingsSelfReferencing = new JsonSerializerSettings(){ ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        /// <summary>
        /// Hashes an <see cref="HttpRequest"/> using the provided <see cref="HashAlgorithm"/>
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/></param>
        /// <param name="hashAlgorithm"><see cref="HashAlgorithm"/></param>
        /// <returns>Request hash as <see langword="string"/></returns>
        public static async Task<string> GetRequestsDataHash(HttpRequest request, HashAlgorithm hashAlgorithm)
        {
            List<object> requestsData = new List<object>();

            if (request.ContentLength.HasValue
                && request.Body != null
                && request.Body.CanRead
                && request.Body.CanSeek)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    request.Body.Position = 0;
                    await request.Body.CopyToAsync(memoryStream);
                    requestsData.Add(memoryStream.ToArray());
                }
            }
                
            if (request.HasFormContentType && request.Form != null)
            {
                requestsData.Add(request.Form);
            }

            if (request.Path.HasValue)
            {
                RouteValueDictionary routeValues = request.RouteValues;
                string action = (string)routeValues["action"];
                string controller = (string)routeValues["controller"];
                requestsData.Add($"/{controller}/{action}");
            }

            return GetHash(hashAlgorithm, requestsData);
        }

        /// <summary>
        /// Hashes an <see langword="object"/> using the provided <see cref="HashAlgorithm"/>
        /// </summary>
        /// <param name="hashAlgorithm"><see cref="HashAlgorithm"/></param>
        /// <param name="obj">The <see langword="object"/> to hash</param>
        /// <returns>Hash <see langword="string"/></returns>
        public static string GetHash(HashAlgorithm hashAlgorithm, object obj)
        {
            string serializedObject = Serialize(obj);
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(serializedObject));
            StringBuilder builder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return builder.ToString();
        }

        public static string Serialize(object obj)
        {
            string json = JsonConvert.SerializeObject(obj, _jsonSerializerSettingsSelfReferencing);
            return json;
        }

        public static T DeSerialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
