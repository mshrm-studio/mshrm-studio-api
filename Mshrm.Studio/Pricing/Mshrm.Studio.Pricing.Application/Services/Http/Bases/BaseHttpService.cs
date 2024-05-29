using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Bases
{
    /// <summary>
    /// Base Http service
    /// </summary>
    public class BaseHttpService
    {
        /// <summary>
        /// The web client used to make Http calls
        /// </summary>
        protected readonly HttpClient _httpClient;

        /// <summary>
        /// Base Http service
        /// </summary>
        /// <param name="client">Web client</param>
        public BaseHttpService(HttpClient client)
        {
            _httpClient = client;
        }

        /// <summary>
        /// Send an HTTP GET request
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="url">The URL to make request to</param>
        /// <param name="useAuthentication">If authentication is required</param>
        /// <param name="additionalHeaders">Any additional headers</param>
        /// <returns>Type T</returns>
        /// <exception cref="Exception">Thrown if request is not successful</exception>
        protected async Task<T> GetAsync<T>(string url, bool useAuthentication, List<KeyValuePair<string, string>>? additionalHeaders = null) where T : class
        {
            await AddHeadersAsync(useAuthentication, additionalHeaders);

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"[{response.StatusCode}]:{response.ReasonPhrase}");
            }

            if (typeof(T) == typeof(string))
            {
                return content as T;
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// Send an HTTP POST request
        /// </summary>
        /// <typeparam name="T1">The return type</typeparam>
        /// <typeparam name="T2">The model type to send</typeparam>
        /// <param name="uri">The URL to make request to</param>
        /// <param name="model">The data to send</param>
        /// <param name="useAuthentication">If authentication is required</param>
        /// <param name="additionalHeaders">Any additional headers</param>
        /// <param name="authenticationScheme"></param>
        /// <returns>Type T</returns>
        protected async Task<T1> PostAsync<T1, T2>(string uri, T2 model, bool useAuthentication, List<KeyValuePair<string, string>>? additionalHeaders = null,
            string authenticationScheme = "Bearer")
            where T1 : class
        {
            await AddHeadersAsync(useAuthentication, additionalHeaders);

            var serializedObject = JsonConvert.SerializeObject(model);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"[{response.StatusCode}]:{response.ReasonPhrase}");
            }

            var responseAsString = await response?.Content?.ReadAsStringAsync();
            if (typeof(T1) == typeof(string))
            {
                return responseAsString as T1;
            }

            return JsonConvert.DeserializeObject<T1>(responseAsString);
        }

        /// <summary>
        /// Send an HTTP PATCH request
        /// </summary>
        /// <typeparam name="T1">The return type</typeparam>
        /// <typeparam name="T2">The model type to send</typeparam>
        /// <param name="uri">The URL to make request to</param>
        /// <param name="model">The data to send</param>
        /// <param name="useAuthentication">If authentication is required</param>
        /// <param name="additionalHeaders">Any additional headers</param>
        /// <returns>Type T</returns>
        protected async Task<T1> PatchAsync<T1, T2>(string uri, T2 model, bool useAuthentication, List<KeyValuePair<string, string>>? additionalHeaders = null)
            where T1 : class
        {
            await AddHeadersAsync(useAuthentication, additionalHeaders);

            var serializedObject = JsonConvert.SerializeObject(model);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync(uri, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"[{response.StatusCode}]:{response.ReasonPhrase}");
            }

            var responseAsString = await response?.Content?.ReadAsStringAsync();
            if (typeof(T1) == typeof(string))
            {
                return responseAsString as T1;
            }

            return JsonConvert.DeserializeObject<T1>(responseAsString);
        }

        #region Authentication

        /// <summary>
        /// Gets the headers to make authenticated call 
        /// </summary>
        /// <returns>Http headers</returns>
        public virtual async Task<List<KeyValuePair<string, string>>> GetAuthenticatedHeadersAsync()
        {
            var token = await GetAccessTokenAsync();

            return new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Content-Type", "application/json"),
                new KeyValuePair<string, string>("Authorization", token)
            };
        }

        /// <summary>
        /// Get an access token - override mandatory
        /// </summary>
        /// <returns>Access token</returns>
        protected virtual async Task<string> GetAccessTokenAsync()
        {
            return await Task.FromResult(string.Empty);
        }

        #endregion

        #region Helpers

        private async Task AddHeadersAsync(bool useAuthentication, List<KeyValuePair<string, string>>? additionalHeaders = null, string authenticationScheme = "Bearer")
        {
            // Ensure headers is not null
            additionalHeaders = (additionalHeaders == null) ? new List<KeyValuePair<string, string>>() : additionalHeaders;

            // Add authentication headers
            if (useAuthentication)
            {
                await AddAuthenticationHeadersAsync(additionalHeaders);
            }

            // Add additional headers
            foreach (var header in additionalHeaders)
            {
                if (header.Key == "Authorization")
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationScheme, header.Value);
                else
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        private async Task<List<KeyValuePair<string, string>>> AddAuthenticationHeadersAsync(List<KeyValuePair<string, string>>? additionalHeaders = null, string authenticationScheme = "Bearer")
        {
            // Ensure headers is not null
            additionalHeaders = (additionalHeaders == null) ? new List<KeyValuePair<string, string>>() : additionalHeaders;

            // Get auth headers and add them for headers of request
            var authHeaders = await GetAuthenticatedHeadersAsync();
            additionalHeaders.AddRange(authHeaders);

            return additionalHeaders;
        }

        #endregion
    }
}
