namespace RiraProject.Users.Application.DTOs;

public record CreateUserDto(
    string FirstName,
    string LastName,
    string NationalCode,
    DateTime BirthDate);
