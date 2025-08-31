namespace LibraryManagement.Core.Models
{
    public class AuthorQueryObject
    {
        public string? Name { get; set; }
        public string? SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}