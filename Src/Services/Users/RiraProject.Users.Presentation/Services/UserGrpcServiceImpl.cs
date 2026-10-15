using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RiraProject.Users.Application.DTOs;
using RiraProject.Users.Application.Services;
using RiraProject.Users.Presentation.Common;
using RiraProject.Users.Presentation.Protos;

namespace RiraProject.Users.Presentation.Services;

public class UserGrpcServiceImpl : UserGrpcService.UserGrpcServiceBase
{
    private readonly IUserService _userService;

    public UserGrpcServiceImpl(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<UserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var dto = new CreateUserDto(
            request.FirstName,
            request.LastName,
            request.NationalCode,
            request.BirthDate.ToDateTime());

        var result = await _userService.CreateAsync(dto, context.CancellationToken);
        if (result.IsFailure)
        {
            throw result.Error.ToRpcException();
        }

        return MapToProto(result.Value);
    }

    public override async Task<UserResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "شناسه کاربر نامعتبر است."));
        }

        var result = await _userService.GetByIdAsync(id, context.CancellationToken);
        if (result.IsFailure)
        {
            throw result.Error.ToRpcException();
        }

        return MapToProto(result.Value);
    }

    public override async Task<GetAllUsersResponse> GetAllUsers(Empty request, ServerCallContext context)
    {
        var result = await _userService.GetAllAsync(context.CancellationToken);
        if (result.IsFailure)
        {
            throw result.Error.ToRpcException();
        }

        var response = new GetAllUsersResponse();
        response.Users.AddRange(result.Value.Select(MapToProto));

        return response;
    }

    public override async Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "شناسه کاربر نامعتبر است."));
        }

        var dto = new UpdateUserDto(
            id,
            request.FirstName,
            request.LastName,
            request.NationalCode,
            request.BirthDate.ToDateTime());

        var result = await _userService.UpdateAsync(dto, context.CancellationToken);
        if (result.IsFailure)
        {
            throw result.Error.ToRpcException();
        }

        return MapToProto(result.Value);
    }

    public override async Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "شناسه کاربر نامعتبر است."));
        }

        var result = await _userService.DeleteAsync(id, context.CancellationToken);
        if (result.IsFailure)
        {
            throw result.Error.ToRpcException();
        }

        return new Empty();
    }

    private static UserResponse MapToProto(UserDto dto)
    {
        return new UserResponse
        {
            Id = dto.Id.ToString(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalCode = dto.NationalCode,
            BirthDate = Timestamp.FromDateTime(DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc))
        };
    }
}