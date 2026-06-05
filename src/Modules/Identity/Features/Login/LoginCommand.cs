using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthTokensDto>>;

public sealed record AuthTokensDto(string AccessToken, string RefreshToken, int ExpiresIn);
