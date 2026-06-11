using CareerTracker.Identity.Dtos;
using CareerTracker.Kernel;
using Dapper;
using MediatR;
using System.Data;

namespace CareerTracker.Identity.Features.GetUserList;

public class GetUsersListHandler : IRequestHandler<GetUsersListQuery, Result<PagedResult<UserListItemDto>>>
{
    private readonly IDbConnection _db;

    // 👈 Один запрос с динамической фильтрацией. Быстро и эффективно.
    private const string Sql = @"
        WITH FilteredUsers AS (
            SELECT ""Id"", ""Email"", ""Role"", ""IsActive"", ""IsDeleted"", ""CreatedAt""
            FROM ""Users""
            WHERE 
                (@Status = 'All') OR
                (@Status = 'Active' AND ""IsActive"" = TRUE AND ""IsDeleted"" = FALSE) OR
                (@Status = 'Inactive' AND ""IsActive"" = FALSE AND ""IsDeleted"" = FALSE) OR
                (@Status = 'Deleted' AND ""IsDeleted"" = TRUE)
        )
        SELECT ""Id"", ""Email"", ""Role"", ""IsActive"", ""IsDeleted"", ""CreatedAt""
        FROM FilteredUsers
        ORDER BY ""CreatedAt"" DESC
        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;

        SELECT COUNT(*) FROM FilteredUsers;
    ";

    public GetUsersListHandler(IDbConnection db) => _db = db;

    public async Task<Result<PagedResult<UserListItemDto>>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
    {
        var offset = (request.Page - 1) * request.PageSize;
        var limit = request.PageSize;
        var statusStr = request.Status.ToString();

        using var multi = await _db.QueryMultipleAsync(Sql, new
        {
            Status = statusStr,
            Offset = offset,
            Limit = limit
        });

        var items = await multi.ReadAsync<UserListItemDto>();
        var totalCount = await multi.ReadFirstAsync<int>();

        return Result<PagedResult<UserListItemDto>>.Success(new PagedResult<UserListItemDto>(items, totalCount));
    }
}
