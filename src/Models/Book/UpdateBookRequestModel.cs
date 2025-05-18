namespace Models.Book
{
    public class UpdateBookRequestModel
    {
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
