using HackathonDataAnalysis.Auth.Models;
using HackathonDataAnalysis.Domain.Dto;

namespace HackathonDataAnalysis.Auth.Interfaces;

public interface IAuthService
{
    Task<LoginDto?> Token(LoginServiceRequest request, CancellationToken cancellationToken);
}