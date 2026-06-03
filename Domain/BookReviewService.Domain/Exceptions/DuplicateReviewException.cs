using BookReviewService.Domain;

namespace BookReviewService.Domain.Exceptions;

public class DuplicateReviewException(User user, Books book)
    : InvalidOperationException($"User '{user.UserName.Value}' has already reviewed the book '{book.NameBook.Value}'.")
{
    public User User => user;
    public Books Book => book;
}