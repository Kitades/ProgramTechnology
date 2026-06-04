using BookReviewService.Domain.Base;
using BookReviewService.Domain.Exceptions;
using BookReviewService.ValueObjects;

namespace BookReviewService.Domain;

public class User : Entity<Guid>
{
    private readonly ICollection<ReviewBook> _reviews = [];

    public Username UserName { get; private set; }

    public IReadOnlyCollection<ReviewBook> Reviews => _reviews.ToList().AsReadOnly();

    protected User()
    {
        UserName = null!;
    }

    public User(Guid id, Username username) : base(id)
    {
        UserName = username ?? throw new ArgumentNullValueException(nameof(username));
    }

    public User(Username username) : this(Guid.NewGuid(), username) { }

    public bool ChangeUsername(Username newUsername)
    {
        if (newUsername == null) throw new ArgumentNullValueException(nameof(newUsername));
        if (UserName == newUsername) return false;
        UserName = newUsername;
        return true;
    }

    public ReviewBook ReviewBook(Books book, CommentText comment, LikeValue like)
    {
        var review = book.AddReview(this, comment, like);
        _reviews.Add(review);
        return review;
    }

    public void DeleteReview(ReviewBook review)
    {
        if (review.User != this) throw new AnotherUserDeleteReviewException(review, this);
        if (!_reviews.Contains(review)) throw new InvalidOperationException("Review not owned by this user.");
        _reviews.Remove(review);
        review.Book.RemoveReview(review);
    }
}