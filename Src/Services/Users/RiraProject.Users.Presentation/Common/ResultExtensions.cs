using Grpc.Core;
using RiraProject.Framework.Domain.Results;

namespace RiraProject.Users.Presentation.Common;

public static class ResultExtensions
{
    public static RpcException ToRpcException(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCode.NotFound,
            ErrorType.Validation => StatusCode.InvalidArgument,
            ErrorType.Conflict => StatusCode.AlreadyExists,
            ErrorType.Unauthorized => StatusCode.Unauthenticated,
            _ => StatusCode.Internal
        };

        return new RpcException(new Status(statusCode, $"{error.Code}: {error.Description}"));
    }
}
