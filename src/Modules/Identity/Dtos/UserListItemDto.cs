namespace CareerTracker.Identity.Dtos;

public record UserListItemDto(int Id,
    string Email,
    string Role,
    bool IsActive,
    bool IsDeleted,
    DateTime CreatedAt
);
