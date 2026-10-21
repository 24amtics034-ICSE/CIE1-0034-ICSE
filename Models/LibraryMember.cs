namespace LibraryManagementMVC.Models;

public class LibraryMember
{
    public int MemberId { get; set; }

    [Required, StringLength(100)]
    public string MemberName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Department { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Range(1, int.MaxValue, ErrorMessage = "Enter a valid Book ID.")]
    public int BookId { get; set; }

    [Required, DataType(DataType.Date)]
    public DateTime IssueDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime? ReturnDate { get; set; }

    // The date by which the book should be returned. Kept separately from ReturnDate,
    // which represents the actual/entered return date in this simple assignment.
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);
}
