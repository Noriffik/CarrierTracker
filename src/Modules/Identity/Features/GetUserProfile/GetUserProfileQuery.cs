using CareerTracker.Identity.Dtos;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.GetUserProfile;

public sealed record GetUserProfileQuery(int UserId) : IRequest<Result<UserProfileDto>>;
