namespace RiraProject.Users.Application.DTOs;

public record UpdateUserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalCode,
    DateTime BirthDate);