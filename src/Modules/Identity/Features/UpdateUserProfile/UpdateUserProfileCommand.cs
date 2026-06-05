using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.UpdateUserProfile;
public sealed record UpdateUserProfileCommand(int UserId,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? City,
    Dictionary<string, object>? Attributes) : IRequest<Result>;
