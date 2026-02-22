using System.Net.Http.Json;
using HackathonDataAnalysis.Auth.Interfaces;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HackathonDataAnalysis.Auth.Services;

public class AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger) : IAuthService
{
    public async Task<LoginDto?> Token(LoginServiceRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Requesting token from auth service for client {ClientId}", request.ClientId);
        
        var response = await httpClient.PostAsJsonAsync("token", request, cancellationToken: cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to retrieve token from auth service. Status code: {StatusCode}", response.StatusCode);
            return null;
        }
        
        logger.LogInformation("Token has been successfully retrieved from auth service.");
        var loginDto = await response.Content.ReadFromJsonAsync<LoginDto>(cancellationToken: cancellationToken);
        return loginDto;
    }
}