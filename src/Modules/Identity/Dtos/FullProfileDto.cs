using CareerTracker.Identity.Domain;

namespace CareerTracker.Identity.Dtos;

public record FullProfileDto(int UserId, string Email, UserRole Role,
    string? FirstName, string? LastName, string? PhoneNumber, string? City,
    string? Attributes, DateTime? CreatedAt);
