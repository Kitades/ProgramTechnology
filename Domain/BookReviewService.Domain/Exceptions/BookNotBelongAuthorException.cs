using BookReviewService.Domain;

namespace BookReviewService.Domain.Exceptions;

public class BookNotBelongAuthorException(Books book, AuthorBook author)
    : InvalidOperationException($"The book '{book.NameBook}' is not authored by '{author.Username}' (book id = {book.Id}).")
{
    public Books Book => book;
    public AuthorBook Author => author;
}