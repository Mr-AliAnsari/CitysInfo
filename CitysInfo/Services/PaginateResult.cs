
namespace CitysInfo.Services
{
    public class PaginateResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PaginationMetadata? Metadata { get; set; }
        public PaginateResult(IEnumerable<T> items, PaginationMetadata paginationMetadata)
        {
            Items = items;
            Metadata = paginationMetadata;
        }
    }
    public class Result<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public Result(T data)
        {
            Data = data; 
            IsSuccess = true;
        }
        public Result(string error)
        {
            Error = error;
            IsSuccess = false;
        }
    }
}
