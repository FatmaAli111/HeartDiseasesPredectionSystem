using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using Models.DTOs;
using Newtonsoft.Json;

namespace IdentityManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;
      

        public ChatBotController(HttpClient httpClient , IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetValue<string>("OpenApi:ApiKey");
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateContent([FromBody] PromptRequestDto request)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            var body = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = request.Prompt }
                    }
                }
            }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, httpContent);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error calling Gemini API");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }
    }



}


