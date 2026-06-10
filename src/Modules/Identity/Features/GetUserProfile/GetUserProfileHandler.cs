using CareerTracker.Identity.Domain;
using CareerTracker.Identity.Dtos;
using CareerTracker.Kernel;
using Dapper;
using MediatR;
using System.Data;

namespace CareerTracker.Identity.Features.GetUserProfile;

public sealed class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, Result<UserProfileDto>>
{
    private readonly IDbConnection _db;
    private const string Sql = @"
        SELECT u.""Id"", u.""Email"", u.""Role"", u.""IsActive"", u.""CreatedAt""
        FROM ""Users"" u
        WHERE u.""Id"" = @UserId AND u.""IsActive"" = TRUE
        ";

    public GetUserProfileHandler(IDbConnection db) => _db = db;

    public async Task<Result<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _db.QueryFirstOrDefaultAsync<UserProfileDto>(Sql, new { request.UserId });

        return user is not null
            ? Result<UserProfileDto>.Success(user)
            : Result<UserProfileDto>.Failure(ValidationErrors.UserNotFound);
    }
}
