namespace LibraryManagementMVC.Controllers;

public class LibraryController : Controller
{
    // In-memory sample data. Data resets when the application restarts.
    private static readonly List<Book> Books =
    [
        new Book { BookId = 1, Title = "The C Programming Language", Author = "Brian W. Kernighan", Category = "Programming", Price = 550m, AvailableCopies = 4 },
        new Book { BookId = 2, Title = "Clean Code", Author = "Robert C. Martin", Category = "Software Engineering", Price = 750m, AvailableCopies = 3 },
        new Book { BookId = 3, Title = "Introduction to Algorithms", Author = "Thomas H. Cormen", Category = "Computer Science", Price = 1200m, AvailableCopies = 2 },
        new Book { BookId = 4, Title = "Database System Concepts", Author = "Abraham Silberschatz", Category = "Database", Price = 900m, AvailableCopies = 5 }
    ];

    private static readonly List<LibraryMember> IssuedBooks = new();
    private static int _nextMemberId = 1;

    public IActionResult Index()
    {
        ViewBag.Books = Books;
        return View(IssuedBooks.OrderByDescending(m => m.IssueDate).ToList());
    }

    [HttpGet]
    public IActionResult IssueBook()
    {
        ViewBag.Books = Books;
        return View(new LibraryMember
        {
            IssueDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(14)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IssueBook(LibraryMember member)
    {
        var book = Books.FirstOrDefault(b => b.BookId == member.BookId);
        if (book is null)
            ModelState.AddModelError(nameof(member.BookId), "The selected Book ID does not exist.");
        else if (book.AvailableCopies <= 0)
            ModelState.AddModelError(nameof(member.BookId), "No copies of this book are currently available.");

        if (member.IssueDate.Date > DateTime.Today.AddDays(1))
            ModelState.AddModelError(nameof(member.IssueDate), "Issue date cannot be in the future.");

        if (member.ReturnDate.HasValue && member.ReturnDate.Value.Date < member.IssueDate.Date)
            ModelState.AddModelError(nameof(member.ReturnDate), "Return date cannot be earlier than issue date.");

        if (!ModelState.IsValid)
        {
            ViewBag.Books = Books;
            return View(member);
        }

        member.MemberId = _nextMemberId++;
        member.DueDate = member.IssueDate.Date.AddDays(14);
        IssuedBooks.Add(member);
        book!.AvailableCopies--;

        TempData["Success"] = $"Book '{book.Title}' was issued to {member.MemberName}.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult BookDetails(int id)
    {
        var member = IssuedBooks.FirstOrDefault(m => m.MemberId == id);
        if (member is null)
            return NotFound("Issued-book record was not found.");

        ViewBag.Book = Books.FirstOrDefault(b => b.BookId == member.BookId);
        return View(member);
    }

    public IActionResult About() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult MarkReturned(int id)
    {
        var member = IssuedBooks.FirstOrDefault(m => m.MemberId == id);
        if (member is null)
            return NotFound();

        if (!member.ReturnDate.HasValue || member.ReturnDate.Value.Date > DateTime.Today)
        {
            member.ReturnDate = DateTime.Today;
            var book = Books.FirstOrDefault(b => b.BookId == member.BookId);
            if (book is not null)
                book.AvailableCopies++;
        }

        TempData["Success"] = $"Book returned by {member.MemberName}.";
        return RedirectToAction(nameof(Index));
    }
}
