using RiraProject.Framework.Domain.Results;
using RiraProject.Users.Application.DTOs;
using RiraProject.Users.Domain.Entities;
using RiraProject.Users.Domain.Repository;
using RiraProject.Users.Domain.UserError;
using RiraProject.Users.Domain.ValueObjects;

namespace RiraProject.Users.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var firstNameResult = FirstName.Create(dto.FirstName);
        if (firstNameResult.IsFailure) return Result.Failure<UserDto>(firstNameResult.Error);

        var lastNameResult = LastName.Create(dto.LastName);
        if (lastNameResult.IsFailure) return Result.Failure<UserDto>(lastNameResult.Error);

        var nationalCodeResult = NationalCode.Create(dto.NationalCode);
        if (nationalCodeResult.IsFailure) return Result.Failure<UserDto>(nationalCodeResult.Error);

        var birthDateResult = BirthDate.Create(dto.BirthDate);
        if (birthDateResult.IsFailure) return Result.Failure<UserDto>(birthDateResult.Error);

        var exists = await _userRepository.ExistsByNationalCodeAsync(nationalCodeResult.Value, cancellationToken);
        if (exists)
            return Result.Failure<UserDto>(UserErrors.NationalCodeAlreadyExists);

        var user = User.Create(
            UserId.New(),
            firstNameResult.Value,
            lastNameResult.Value,
            nationalCodeResult.Value,
            birthDateResult.Value).Value;

        await _userRepository.InsertAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(MapToDto(user));
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(UserId.From(id), cancellationToken);
        if (user is null)
            return Result.Failure<UserDto>(UserErrors.NotFound);

        return Result.Success(MapToDto(user));
    }

    public async Task<Result<IReadOnlyList<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        var dtos = users.Select(MapToDto).ToList();
        return Result.Success<IReadOnlyList<UserDto>>(dtos);
    }

    public async Task<Result<UserDto>> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(UserId.From(dto.Id), cancellationToken);
        if (user is null)
            return Result.Failure<UserDto>(UserErrors.NotFound);

        var firstNameResult = FirstName.Create(dto.FirstName);
        if (firstNameResult.IsFailure) return Result.Failure<UserDto>(firstNameResult.Error);

        var lastNameResult = LastName.Create(dto.LastName);
        if (lastNameResult.IsFailure) return Result.Failure<UserDto>(lastNameResult.Error);

        var nationalCodeResult = NationalCode.Create(dto.NationalCode);
        if (nationalCodeResult.IsFailure) return Result.Failure<UserDto>(nationalCodeResult.Error);

        var birthDateResult = BirthDate.Create(dto.BirthDate);
        if (birthDateResult.IsFailure) return Result.Failure<UserDto>(birthDateResult.Error);

        if (user.NationalCode.Value != nationalCodeResult.Value.Value)
        {
            var exists = await _userRepository.ExistsByNationalCodeAsync(nationalCodeResult.Value, cancellationToken);
            if (exists)
                return Result.Failure<UserDto>(UserErrors.NationalCodeAlreadyExists);
        }

        user.Update(firstNameResult.Value, lastNameResult.Value, nationalCodeResult.Value, birthDateResult.Value);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(MapToDto(user));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(UserId.From(id), cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.NotFound);

        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static UserDto MapToDto(User user) =>
        new(user.Id.Value, user.FirstName.Value, user.LastName.Value, user.NationalCode.Value, user.BirthDate.Value);
}