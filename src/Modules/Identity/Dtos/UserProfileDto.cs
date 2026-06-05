namespace CareerTracker.Identity.Dtos;

public record UserProfileDto(int Id, string Email,
    string Role,      // Строковое представление роли для UI
    bool IsActive,
    DateTime CreatedAt
);
