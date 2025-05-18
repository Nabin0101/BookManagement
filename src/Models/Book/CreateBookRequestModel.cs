using System.ComponentModel.DataAnnotations;

namespace Models.Book
{
    public class CreateBookRequestModel
    {
        [Required]
        public string BookName { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
    }
}
