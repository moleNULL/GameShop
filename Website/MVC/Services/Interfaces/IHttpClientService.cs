﻿namespace MVC.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod httpMethod, TRequest? content);
    }
}
