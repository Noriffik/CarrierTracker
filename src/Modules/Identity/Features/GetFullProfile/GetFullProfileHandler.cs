using CareerTracker.Kernel;
using Dapper;
using MediatR;
using System.Data;

namespace CareerTracker.Identity.Features.GetFullProfile;

public class GetFullProfileHandler : IRequestHandler<GetFullProfileQuery, Result<FullProfileDto>>
{
    private readonly IDbConnection _db;
    private const string Sql = """
        SELECT 
            u.Id as UserId, u.Email, u.Role, u.CreatedAt as UserCreatedAt,
            p.FirstName, p.LastName, p.PhoneNumber, p.City, p.Attributes, p.CreatedAt as ProfileCreatedAt
        FROM Users u
        LEFT JOIN UserProfiles p ON p.UserId = u.Id
        WHERE u.Id = @UserId AND u.IsActive = 1
        """;

    public GetFullProfileHandler(IDbConnection db) => _db = db;

    public async Task<Result<FullProfileDto>> Handle(GetFullProfileQuery request, CancellationToken ct)
    {
        var result = await _db.QueryFirstOrDefaultAsync<FullProfileDto>(Sql, new { request.UserId });

        return result is not null
            ? Result<FullProfileDto>.Success(result)
            : Result<FullProfileDto>.Failure("Профиль не найден");
    }
}