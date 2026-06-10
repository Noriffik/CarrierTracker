using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Identity.Features.UpdateUserProfile;

public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, Result>
{
    private readonly UserDbContext _context;

    public UpdateUserProfileHandler(UserDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
        {
            profile = UserProfile.Create(request.UserId, request.FirstName, request.LastName);
            _context.Profiles.Add(profile);
        }
        else
        {
            profile.UpdateContact(request.PhoneNumber, request.City);

            if (profile.FirstName != request.FirstName || profile.LastName != request.LastName)
            {
                profile.UpdateName(request.FirstName, request.LastName); 
            }
        }

        if (request.Attributes is not null)
        {
            foreach (var attr in request.Attributes)
                profile.SetAttribute(attr.Key, attr.Value);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
