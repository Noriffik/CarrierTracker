using CareerTracker.Identity.Domain;
using CareerTracker.Identity.Features.GetUserProfile;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.GetFullProfile;

public record GetFullProfileQuery(int UserId) : IRequest<Result<FullProfileDto>>;

public record FullProfileDto(int UserId, string Email, UserRole Role,
    string FirstName, string LastName, string? PhoneNumber, string? City,
    Dictionary<string, object> Attributes, DateTime CreatedAt);
