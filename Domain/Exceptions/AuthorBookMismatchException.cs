namespace Booksworm.Domain.Exceptions;

public class AuthorBookMismatchException : InvalidOperationException
{
    public Guid AuthorId { get; }
    public Guid BookId { get; }

    public AuthorBookMismatchException(Guid authorId, Guid bookId)
        : base($"The book with ID {bookId} does not belong to the author with ID {authorId}.")
    {
        AuthorId = authorId;
        BookId = bookId;
    }
}