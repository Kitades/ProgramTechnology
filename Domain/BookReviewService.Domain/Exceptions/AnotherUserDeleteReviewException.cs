using BookReviewService.Domain;

namespace BookReviewService.Domain.Exceptions;

public class AnotherUserDeleteReviewException(ReviewBook review, User user)
    : InvalidOperationException($"The user '{user.UserName.Value}' can't delete the review (id = {review.Id}) of user '{review.User.UserName.Value}'.")
{
    public ReviewBook Review => review;
    public User User => user;
}