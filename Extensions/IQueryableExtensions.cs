using LibraryManagement.Core.Models;

namespace LibraryManagement.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<Book> ApplyFiltering(this IQueryable<Book> query, BookQueryObject bookQueryObj)
        {
            if (!string.IsNullOrWhiteSpace(bookQueryObj.Title))
                query = query.Where(b => b.Title.Contains(bookQueryObj.Title));

            if (!string.IsNullOrWhiteSpace(bookQueryObj.AuthorName))
                query = query.Where(b => b.Author.Name.Contains(bookQueryObj.AuthorName));

            return query;
        }

        public static IQueryable<Book> ApplySorting(this IQueryable<Book> query, BookQueryObject bookQueryObj)
        {
            if (!string.IsNullOrWhiteSpace(bookQueryObj.SortBy))
            {
                if (bookQueryObj.SortBy.Equals("Title", StringComparison.CurrentCultureIgnoreCase))
                    query = bookQueryObj.IsSortAscending ?
                        query.OrderBy(b => b.Title) : query.OrderByDescending(b => b.Title);
            }

            return query;
        }

        public static IQueryable<Book> ApplyPagination(this IQueryable<Book> query, BookQueryObject bookQueryObj)
        {
            if (bookQueryObj.PageNumber > 0 && bookQueryObj.PageSize > 0)
                query = query
                    .Skip((bookQueryObj.PageNumber - 1) * bookQueryObj.PageSize)
                    .Take(bookQueryObj.PageSize);

            return query;
        }
    }
}