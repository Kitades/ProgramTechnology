using BookReviewService.Domain.Base;
using BookReviewService.Domain.Exceptions;
using BookReviewService.ValueObjects;

namespace BookReviewService.Domain;

public class ReviewBook : Entity<Guid>
{
    public CommentText CommentBook { get; private set; }
    public LikeValue LikeBook { get; private set; }
    public DateTime ReviewDate { get; }

    public Books Book { get; private set; }
    public User User { get; private set; }

    protected ReviewBook()
    {
        CommentBook = null!;
        LikeBook = null!;
        Book = null!;
        User = null!;
    }

    public ReviewBook(Books book, User user, CommentText comment, LikeValue like)
        : this(Guid.NewGuid(), book, user, comment, like, DateTime.UtcNow) { }

    public ReviewBook(Guid id, Books book, User user, CommentText comment, LikeValue like, DateTime reviewDate)
        : base(id)
    {
        Book = book ?? throw new ArgumentNullValueException(nameof(book));
        User = user ?? throw new ArgumentNullValueException(nameof(user));
        CommentBook = comment ?? throw new ArgumentNullValueException(nameof(comment));
        LikeBook = like ?? throw new ArgumentNullValueException(nameof(like));
        ReviewDate = reviewDate;
    }

    public bool UpdateComment(CommentText newComment)
    {
        if (newComment == null) throw new ArgumentNullValueException(nameof(newComment));
        if (CommentBook == newComment) return false;
        CommentBook = newComment;
        return true;
    }

    public bool UpdateLike(LikeValue newLike)
    {
        if (newLike == null) throw new ArgumentNullValueException(nameof(newLike));
        if (LikeBook == newLike) return false;
        LikeBook = newLike;
        return true;
    }
}