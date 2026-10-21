namespace LibraryManagementMVC.Models;

public class Book
{
    public int BookId { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Category { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [Range(0, 10000)]
    public int AvailableCopies { get; set; }
}
