using Azure;

namespace LibraryManagement.Core.Models
{
    public class OperationResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public int? ErrorCode { get; init; }

        public static OperationResult Succeed()
        {
            return new() { Success = true };
        }

        public static OperationResult Fail(string? errorMessage = null, int? errorCode = null)
        {
            return new() { Success = false, ErrorMessage = errorMessage, ErrorCode = errorCode };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; init; }

        public static OperationResult<T> Succeed(T data)
        {
            return new() { Success = true, Data = data };
        }

        public static new OperationResult<T> Fail(string? errorMessage = null, int? errorCode = null)
        {
            return new() { Success = false, Data = default, ErrorMessage = errorMessage, ErrorCode = errorCode };
        }
    }
}