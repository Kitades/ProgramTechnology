using BookReviewService.Domain.Base;
using BookReviewService.Domain.Exceptions;
using BookReviewService.ValueObjects;

namespace BookReviewService.Domain;

public class AuthorBook : Entity<Guid>
{
    private readonly ICollection<Books> _books = [];

    public Username Username { get; private set; }

    public IReadOnlyCollection<Books> Books => _books.ToList().AsReadOnly();

    protected AuthorBook()
    {
         Username = null!;
    } // for EF

    public AuthorBook(Guid id, Username username) : base(id)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
    }

    public AuthorBook(Username username) : this(Guid.NewGuid(), username) { }

    public bool ChangeUsername(Username newUsername)
    {
        if (newUsername == null) throw new ArgumentNullValueException(nameof(newUsername));
        if (Username == newUsername) return false;
        Username = newUsername;
        return true;
    }

    public Books CreateBook(BookName name, BookTitle title, BookText text)
    {
        var book = new Books(this, name, title, text);
        _books.Add(book);
        return book;
    }

    public bool EditBook(Books book, BookName newName, BookTitle newTitle, BookText newText)
    {
        if (book.Author != this) throw new BookNotBelongAuthorException(book, this);
        if (!_books.Contains(book)) throw new BookNotBelongAuthorException(book, this);

        bool edited = book.SetName(newName) || book.SetTitle(newTitle) || book.SetText(newText);
        if (edited) book.SetModificationDate(DateTime.UtcNow);
        return edited;
    }

    public void DeleteBook(Books book)
    {
        if (book.Author != this) throw new BookNotBelongAuthorException(book, this);
        if (!_books.Contains(book)) throw new BookNotBelongAuthorException(book, this);
        _books.Remove(book);
    }
}