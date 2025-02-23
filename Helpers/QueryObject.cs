namespace book_management.Helpers
{
    public class QueryObject
    {
        public string? Query { get; set; } = null;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}