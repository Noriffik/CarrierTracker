using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using Dapper;
using MediatR;
using System.Data;

namespace CareerTracker.Identity.Features.GetFullProfile;

public class GetFullProfileHandler : IRequestHandler<GetFullProfileQuery, Result<FullProfileDto>>
{
    private readonly IDbConnection _db;
    private const string Sql = @"
        SELECT 
            u.""Id"" AS ""UserId"", 
            u.""Email"" AS ""Email"", 
            u.""Role"" AS ""Role"",
            p.""FirstName"" AS ""FirstName"", 
            p.""LastName"" AS ""LastName"", 
            p.""PhoneNumber"" AS ""PhoneNumber"", 
            p.""City"" AS ""City"",
            p.""Attributes"" AS ""Attributes"", 
            u.""CreatedAt"" AS ""CreatedAt""
        FROM ""Users"" u 
        LEFT JOIN ""UserProfiles"" p ON p.""UserId"" = u.""Id""
        WHERE u.""Id"" = @UserId AND u.""IsActive"" = TRUE
    ";

    public GetFullProfileHandler(IDbConnection db) => _db = db;

    public async Task<Result<FullProfileDto>> Handle(GetFullProfileQuery request, CancellationToken ct)
    {
        var result = await _db.QueryFirstOrDefaultAsync<FullProfileDto>(Sql, new { request.UserId });

        return result is not null
            ? Result<FullProfileDto>.Success(result)
            : Result<FullProfileDto>.Failure(ValidationErrors.ProfileNotFound);
    }
}