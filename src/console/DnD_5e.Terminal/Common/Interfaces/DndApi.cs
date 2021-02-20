using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Terminal.Roll;

namespace DnD_5e.Terminal.Common.Interfaces
{
    public interface IDndApi
    {
        Task<RollResponse> FreeRoll(string rollRequest);
    }

    public class DndApi : IDndApi
    {
        private readonly HttpClient _http;

        public DndApi(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:5001/api/");
        }

        public async Task<RollResponse> FreeRoll(string rollRequest)
        {
            var request = rollRequest.Replace("+", "p").Replace("-", "m");
            try
            {
                var response = await _http.GetAsync($"roll/{request}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync(CancellationToken.None);

                return JsonSerializer.Deserialize<RollResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new ApiException(
                        "Your roll request does not appear to be properly formatted. Please try again.");
                }
                throw new ApiException("The D&D service encountered an error processing your request.");
            }
        }
    }
}