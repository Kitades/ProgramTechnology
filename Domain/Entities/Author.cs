using Booksworm.Domain.Abstractions;
using Booksworm.Domain.ValueObjects;

namespace Booksworm.Domain.Entities;

public class Author : Entity<Guid>
{
    private readonly List<Book> _books = new();

    public AuthorName Name { get; private set; }
    public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

    private Author() { } // For EF Core

    public Author(AuthorName name)
        : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public Book CreateBook(BookTitle title, string description)
    {
        var book = new Book(this, title, description);
        _books.Add(book);
        return book;
    }

    public bool EditBook(Book book, BookTitle newTitle, string newDescription)
    {
        if (book.Author != this)
            throw new InvalidOperationException("An author can only edit their own books.");

        if (!_books.Contains(book))
            throw new InvalidOperationException("The specified book does not belong to this author.");

        book.UpdateDetails(newTitle, newDescription);
        return true;
    }

    public bool DeleteBook(Book book)
    {
        if (book.Author != this)
            throw new InvalidOperationException("An author can only delete their own books.");

        if (!_books.Contains(book))
            throw new InvalidOperationException("The specified book does not belong to this author.");

        return _books.Remove(book);
    }
}