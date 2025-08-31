using LibraryManagement.Dtos.AuthorDtos;

namespace LibraryManagement.Dtos.QueryResultDtos
{
    public class AuthorQueryResultDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<AuthorDto> Items { get; set; } = [];
    }
}