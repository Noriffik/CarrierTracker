using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.ResetPassword;

public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Result>;
