using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Identity.Infrastructure;
using CareerTracker.Kernel;
using CareerTracker.Kernel.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Identity.Features.RequestPasswordReset;

public class RequestPasswordResetHandler : IRequestHandler<RequestPasswordResetCommand, Result>
{
    private readonly UserDbContext _context;
    private readonly IEmailSender _emailSender;
    private readonly IPasswordResetTokenGenerator _tokenGenerator;

    public RequestPasswordResetHandler(UserDbContext context, IEmailSender emailSender, IPasswordResetTokenGenerator tokenGenerator)
    {
        _context = context;
        _emailSender = emailSender;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase), cancellationToken);

        if (user is not null && user.IsActive)
        {
            var token = _tokenGenerator.Generate();
            var tokenHash = TokenHasher.Hash(token);
            var resetRequest = PasswordResetRequest.Create(user.Id, tokenHash, TimeSpan.FromHours(1));

            _context.PasswordResetRequests.Add(resetRequest);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailSender.SendPasswordResetEmailAsync(user.Email, token);
        }

        return Result.Success();
    }
}
