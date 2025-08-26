namespace LibraryManagement.Models;

public class BookQueryObject
{
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string SortBy { get; set; } = string.Empty;
    public bool IsSortAscending { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
