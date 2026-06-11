using CareerTracker.Identity.Dtos;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.GetUserList;

public record GetUsersListQuery(UserStatusFilter Status, int Page, int PageSize) : IRequest<Result<PagedResult<UserListItemDto>>>;

public enum UserStatusFilter { Active, Inactive, Deleted, All }
