using BookReviewService.Domain;

namespace BookReviewService.Domain.Exceptions;

public class AnotherUserEditBookException(Books book, User user)
    : InvalidOperationException($"The user '{user.UserName.Value}' can't edit the book '{book.NameBook.Value}' owned by author '{book.Author.Username.Value}' (book id = {book.Id}).")
{
    public Books Book => book;
    public User User => user;
}