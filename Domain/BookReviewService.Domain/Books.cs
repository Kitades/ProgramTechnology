using BookReviewService.Domain.Base;
using BookReviewService.Domain.Exceptions;
using BookReviewService.ValueObjects;

namespace BookReviewService.Domain;

public class Books : Entity<Guid>
{
    private readonly ICollection<ReviewBook> _reviews = [];

    public BookName NameBook { get; private set; }
    public BookTitle TitleBook { get; private set; }
    public BookText TextBook { get; private set; }
    public DateTime CreationDate { get; }
    public DateTime? ModificationDate { get; private set; }

    public AuthorBook Author { get; private set; }

    public IReadOnlyCollection<ReviewBook> Reviews => _reviews.ToList().AsReadOnly();

    protected Books()
    {
        NameBook = null!;
        TitleBook = null!;
        TextBook = null!;
        Author = null!;
    }

    public Books(AuthorBook author, BookName name, BookTitle title, BookText text)
        : this(Guid.NewGuid(), author, name, title, text, DateTime.UtcNow, null) { }

    public Books(Guid id, AuthorBook author, BookName name, BookTitle title, BookText text, DateTime creationDate, DateTime? modificationDate)
        : base(id)
    {
        Author = author ?? throw new ArgumentNullValueException(nameof(author));
        NameBook = name ?? throw new ArgumentNullValueException(nameof(name));
        TitleBook = title ?? throw new ArgumentNullValueException(nameof(title));
        TextBook = text ?? throw new ArgumentNullValueException(nameof(text));
        CreationDate = creationDate;
        if (modificationDate.HasValue && modificationDate < creationDate)
            throw new ArgumentException("Modification date cannot be before creation date.");
        ModificationDate = modificationDate;
    }

    public bool SetName(BookName newName)
    {
        if (newName == null) throw new ArgumentNullValueException(nameof(newName));
        if (NameBook == newName) return false;
        NameBook = newName;
        return true;
    }

    public bool SetTitle(BookTitle newTitle)
    {
        if (newTitle == null) throw new ArgumentNullValueException(nameof(newTitle));
        if (TitleBook == newTitle) return false;
        TitleBook = newTitle;
        return true;
    }

    public bool SetText(BookText newText)
    {
        if (newText == null) throw new ArgumentNullValueException(nameof(newText));
        if (TextBook == newText) return false;
        TextBook = newText;
        return true;
    }

    public void SetModificationDate(DateTime modificationDate)
    {
        if (modificationDate < CreationDate)
            throw new ArgumentException("Modification date cannot be before creation date.");
        ModificationDate = modificationDate;
    }

    public ReviewBook AddReview(User user, CommentText comment, LikeValue like)
    {
        if (_reviews.Any(r => r.User.Id == user.Id))
            throw new DuplicateReviewException(user, this);

        var review = new ReviewBook(this, user, comment, like);
        _reviews.Add(review);
        return review;
    }

    public void RemoveReview(ReviewBook review)
    {
        if (!_reviews.Contains(review))
            throw new InvalidOperationException("Review not found in this book.");
        _reviews.Remove(review);
    }
}