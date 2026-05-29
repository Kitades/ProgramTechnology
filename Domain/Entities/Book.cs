using Booksworm.Domain.Abstractions;
using Booksworm.Domain.ValueObjects;

namespace Booksworm.Domain.Entities;

public class Book : Entity<Guid>
{
    private readonly List<Review> _reviews = new();

    public BookTitle Title { get; private set; }
    public string Description { get; private set; }
    public Author Author { get; private set; }
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

    private Book() { } // For EF Core

    public Book(Author author, BookTitle title, string description)
        : base(Guid.NewGuid())
    {
        Author = author ?? throw new ArgumentNullException(nameof(author));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public void UpdateDetails(BookTitle newTitle, string newDescription)
    {
        Title = newTitle ?? throw new ArgumentNullException(nameof(newTitle));
        Description = newDescription ?? throw new ArgumentNullException(nameof(newDescription));
    }

    public void AddReview(Review review)
    {
        _reviews.Add(review);
    }
}