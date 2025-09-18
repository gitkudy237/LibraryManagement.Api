namespace LibraryManagement.Core.Models
{
    public class OperationResult
    {
        public bool Success { get; }
        public string? ErrorMessage { get; }
        public int? ErroCode { get; }
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; }
    }
}