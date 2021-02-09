using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using DnD_5e.Terminal.Roll;
using Microsoft.Extensions.Configuration;

namespace DnD_5e.Terminal.Common
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
            try
            {
                var response = await _http.GetAsync($"roll/{rollRequest}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync(CancellationToken.None);

                return JsonSerializer.Deserialize<RollResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("The D&D service appears to be unavailable");
            }
        }
    }
}