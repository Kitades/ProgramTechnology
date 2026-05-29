using Booksworm.Domain.Abstractions;
using Booksworm.Domain.ValueObjects;

namespace Booksworm.Domain.Entities;

public class Reader : Entity<Guid>
{
    private readonly List<Review> _reviews = new();
    private readonly List<Comment> _comments = new();

    public Username Username { get; private set; }
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    private Reader() { } // For EF Core

    public Reader(Username username)
        : base(Guid.NewGuid())
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
    }

    public Review AddReview(Book book, Rating rating, string reviewText)
    {
        var review = new Review(this, book, rating, reviewText);
        _reviews.Add(review);
        book.AddReview(review);
        return review;
    }

    public Comment AddComment(Book book, string commentText)
    {
        var comment = new Comment(this, book, commentText);
        _comments.Add(comment);
        return comment;
    }
}