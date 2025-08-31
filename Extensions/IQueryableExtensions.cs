using LibraryManagement.Core.Models;

namespace LibraryManagement.Extensions
{
    public static class IQueryableExtensions
    {
        // Extending IQueryable<Book> here
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
            // just giving defaults in case we have invalid values
            if (bookQueryObj.PageNumber <= 0)
                bookQueryObj.PageNumber = 1;
            if (bookQueryObj.PageSize <= 0)
                bookQueryObj.PageSize = 10;

            query = query
                .Skip((bookQueryObj.PageNumber - 1) * bookQueryObj.PageSize)
                .Take(bookQueryObj.PageSize);

            return query;
        }

        // Extending IQueryable<Author> here
        public static IQueryable<Author> ApplyFiltering(this IQueryable<Author> query, AuthorQueryObject authorQueryObj)
        {
            if (!string.IsNullOrWhiteSpace(authorQueryObj.Name))
                query = query.Where(a => a.Name.Contains(authorQueryObj.Name));

            return query;
        }

        public static IQueryable<Author> ApplySorting(this IQueryable<Author> query, AuthorQueryObject authorQueryObj)
        {
            if (!string.IsNullOrWhiteSpace(authorQueryObj.SortBy))
            {
                if (authorQueryObj.SortBy.Equals("Name", StringComparison.CurrentCultureIgnoreCase))
                    query = authorQueryObj.IsSortAscending ?
                        query.OrderBy(b => b.Name) : query.OrderByDescending(b => b.Name);
            }

            return query;
        }

        public static IQueryable<Author> ApplyPagination(this IQueryable<Author> query, AuthorQueryObject authorQueryObj)
        {
            if (authorQueryObj.PageNumber <= 0)
                authorQueryObj.PageNumber = 1;
            if (authorQueryObj.PageSize <= 0)
                authorQueryObj.PageSize = 10;

            query = query
                .Skip((authorQueryObj.PageNumber - 1) * authorQueryObj.PageSize)
                .Take(authorQueryObj.PageSize);

            return query;
        }

    }
}