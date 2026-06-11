using CareerTracker.Identity.Dtos;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.GetFullProfile;

public record GetFullProfileQuery(int UserId) : IRequest<Result<FullProfileDto>>;
