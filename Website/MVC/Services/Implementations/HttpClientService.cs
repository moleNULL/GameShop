﻿using System.Text;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC.Services.Implementations
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<HttpClientService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod httpMethod, TRequest? content)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var token = await _httpContextAccessor!.HttpContext!.GetTokenAsync("access_token");

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.SetBearerToken(token);
            }

            var httpMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = httpMethod
            };

            if (content is not null)
            {
                httpMessage.Content =
                    new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            var result = await httpClient.SendAsync(httpMessage);

            if (result.IsSuccessStatusCode)
            {
                var resultContent = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponse>(resultContent);

                return response!;
            }

            _logger.LogError($"httpClient return unsuccessful code: {await result.Content.ReadAsStringAsync()}");

            return default !;
        }
    }
}
