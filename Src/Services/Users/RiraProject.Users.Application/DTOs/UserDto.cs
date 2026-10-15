namespace RiraProject.Users.Application.DTOs;

public record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalCode,
    DateTime BirthDate);
