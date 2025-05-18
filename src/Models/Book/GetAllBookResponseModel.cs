namespace Models.Book
{
    public class GetAllBookResponseModel
    {
        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }

    public class BookDto
    {
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
