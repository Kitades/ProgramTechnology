using BookReviewService.Domain;
using BookReviewService.Domain.Repositories.Abstractions;
using BookReviewService.ValueObjects;
using System.Collections.Concurrent;

namespace BookReviewService.DomainApp;

internal class Program
{
    private static readonly IAuthorBookRepository _authorRepo = new InMemoryAuthorRepository();
    private static readonly IBooksRepository _bookRepo = new InMemoryBookRepository();
    private static readonly IUserRepository _userRepo = new InMemoryUserRepository();
    private static readonly IReviewBookRepository _reviewRepo = new InMemoryReviewRepository();

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        // Добавим тестового автора и пользователя для демонстрации
        var defaultAuthor = new AuthorBook(new Username("john_doe"));
        await _authorRepo.AddAsync(defaultAuthor, CancellationToken.None);

        var defaultUser = new User(new Username("jane_smith"));
        await _userRepo.AddAsync(defaultUser, CancellationToken.None);

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== BOOK REVIEW SERVICE ===");
            Console.WriteLine("1. Create book");
            Console.WriteLine("2. Edit book");
            Console.WriteLine("3. View book");
            Console.WriteLine("4. Delete book");
            Console.WriteLine("5. View all books");
            Console.WriteLine("6. Comment book");
            Console.WriteLine("7. Like book (add review with like)");
            Console.WriteLine("8. Exit");
            Console.Write("Choose option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": await CreateBook(); break;
                case "2": await EditBook(); break;
                case "3": await ViewBook(); break;
                case "4": await DeleteBook(); break;
                case "5": await ViewAllBooks(); break;
                case "6": await CommentBook(); break;
                case "7": await LikeBook(); break;
                case "8": exit = true; break;
                default: Console.WriteLine("Invalid option"); break;
            }
            if (!exit) { Console.WriteLine("\nPress any key to continue..."); Console.ReadKey(); }
        }
    }

    static async Task CreateBook()
    {
        Console.Write("Author username: ");
        string authorName = Console.ReadLine()!;
        var author = await _authorRepo.GetByUsernameAsync(authorName, CancellationToken.None);
        if (author == null) { Console.WriteLine("Author not found"); return; }

        Console.Write("Book name: ");
        var bookName = new BookName(Console.ReadLine()!);
        Console.Write("Book title: ");
        var bookTitle = new BookTitle(Console.ReadLine()!);
        Console.Write("Book text: ");
        var bookText = new BookText(Console.ReadLine()!);

        var book = author.CreateBook(bookName, bookTitle, bookText);
        await _bookRepo.AddAsync(book, CancellationToken.None);
        Console.WriteLine($"Book '{book.NameBook.Value}' created with ID {book.Id}");
    }

    static async Task EditBook()
    {
        Console.Write("Book ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var bookId)) { Console.WriteLine("Invalid ID"); return; }
        var book = await _bookRepo.GetByIdAsync(bookId, CancellationToken.None);
        if (book == null) { Console.WriteLine("Book not found"); return; }

        Console.Write("New name (or empty to keep): ");
        string? newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
            book.SetName(new BookName(newName));

        Console.Write("New title (or empty to keep): ");
        string? newTitle = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newTitle))
            book.SetTitle(new BookTitle(newTitle));

        Console.Write("New text (or empty to keep): ");
        string? newText = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newText))
            book.SetText(new BookText(newText));

        await _bookRepo.UpdateAsync(book, CancellationToken.None);
        Console.WriteLine("Book updated.");
    }

    static async Task ViewBook()
    {
        Console.Write("Book ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var bookId)) { Console.WriteLine("Invalid ID"); return; }
        var book = await _bookRepo.GetByIdAsync(bookId, CancellationToken.None);
        if (book == null) { Console.WriteLine("Book not found"); return; }

        Console.WriteLine($"\nID: {book.Id}");
        Console.WriteLine($"Name: {book.NameBook.Value}");
        Console.WriteLine($"Title: {book.TitleBook.Value}");
        Console.WriteLine($"Text: {book.TextBook.Value}");
        Console.WriteLine($"Author: {book.Author.Username.Value}");
        Console.WriteLine($"Creation: {book.CreationDate}");
        Console.WriteLine($"Modified: {book.ModificationDate}");
        Console.WriteLine("\nReviews:");
        foreach (var rev in book.Reviews)
        {
            Console.WriteLine($"  - {rev.User.UserName.Value}: {rev.CommentBook.Value} (like: {rev.LikeBook.Value})");
        }
    }

    static async Task DeleteBook()
    {
        Console.Write("Book ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var bookId)) { Console.WriteLine("Invalid ID"); return; }
        var book = await _bookRepo.GetByIdAsync(bookId, CancellationToken.None);
        if (book == null) { Console.WriteLine("Book not found"); return; }

        await _bookRepo.DeleteAsync(bookId, CancellationToken.None);
        Console.WriteLine("Book deleted.");
    }

    static async Task ViewAllBooks()
    {
        var books = await _bookRepo.GetAllAsync(CancellationToken.None);
        foreach (var book in books)
        {
            Console.WriteLine($"{book.Id} | {book.NameBook.Value} | by {book.Author.Username.Value} | reviews: {book.Reviews.Count}");
        }
    }

    static async Task CommentBook()
    {
        Console.Write("Book ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var bookId)) { Console.WriteLine("Invalid ID"); return; }
        var book = await _bookRepo.GetByIdAsync(bookId, CancellationToken.None);
        if (book == null) { Console.WriteLine("Book not found"); return; }

        Console.Write("Your username: ");
        var username = Console.ReadLine()!;
        var user = await _userRepo.GetByUsernameAsync(username, CancellationToken.None);
        if (user == null) { Console.WriteLine("User not found. Create new? (y/n)"); if (Console.ReadLine()?.ToLower() == "y") { user = new User(new Username(username)); await _userRepo.AddAsync(user, CancellationToken.None); } else return; }

        Console.Write("Comment: ");
        var comment = new CommentText(Console.ReadLine()!);
        Console.Write("Like (1-10): ");
        if (!int.TryParse(Console.ReadLine(), out int likeVal) || likeVal < 1 || likeVal > 10) { Console.WriteLine("Invalid like value. Using 5."); likeVal = 5; }
        var like = new LikeValue(likeVal);

        var review = user.ReviewBook(book, comment, like);
        await _reviewRepo.AddAsync(review, CancellationToken.None);
        Console.WriteLine("Review added.");
    }

    static async Task LikeBook()
    {
        // Like book - аналогично комментарию, просто отдельный пункт, но можно объединить. Сделаем как добавление лайка без комментария?
        // По ТЗ "Like book" – вероятно, отдельная операция. Пусть будет возможность поставить лайк без комментария.
        Console.Write("Book ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var bookId)) { Console.WriteLine("Invalid ID"); return; }
        var book = await _bookRepo.GetByIdAsync(bookId, CancellationToken.None);
        if (book == null) { Console.WriteLine("Book not found"); return; }

        Console.Write("Your username: ");
        var username = Console.ReadLine()!;
        var user = await _userRepo.GetByUsernameAsync(username, CancellationToken.None);
        if (user == null) { Console.WriteLine("User not found. Create new? (y/n)"); if (Console.ReadLine()?.ToLower() == "y") { user = new User(new Username(username)); await _userRepo.AddAsync(user, CancellationToken.None); } else return; }

        Console.Write("Like value (1-10): ");
        if (!int.TryParse(Console.ReadLine(), out int likeVal) || likeVal < 1 || likeVal > 10) { Console.WriteLine("Invalid like value."); return; }
        var like = new LikeValue(likeVal);

        // Для лайка без комментария создаём отзыв с пустым комментарием? Лучше с фиктивным.
        var comment = new CommentText("(like only)");
        var review = user.ReviewBook(book, comment, like);
        await _reviewRepo.AddAsync(review, CancellationToken.None);
        Console.WriteLine("Like added.");
    }
}

// In-memory репозитории (простые реализации для демонстрации)
public class InMemoryAuthorRepository : IAuthorBookRepository
{
    private readonly ConcurrentDictionary<Guid, AuthorBook> _store = new();
    public Task<IEnumerable<AuthorBook>> GetAllAsync(CancellationToken ct, bool asNoTracking = false) => Task.FromResult(_store.Values.AsEnumerable());
    public Task<AuthorBook?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.GetValueOrDefault(id));
    public Task<AuthorBook?> AddAsync(AuthorBook entity, CancellationToken ct) { _store[entity.Id] = entity; return Task.FromResult<AuthorBook?>(entity); }
    public Task<bool> UpdateAsync(AuthorBook entity, CancellationToken ct) { _store[entity.Id] = entity; return Task.FromResult(true); }
    public Task<bool> DeleteAsync(AuthorBook entity, CancellationToken ct) => DeleteAsync(entity.Id, ct);
    public Task<bool> DeleteAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.TryRemove(id, out _));
    public Task<AuthorBook?> GetByUsernameAsync(string username, CancellationToken ct) => Task.FromResult(_store.Values.FirstOrDefault(a => a.Username.Value == username));
}

public class InMemoryBookRepository : IBooksRepository
{
    private readonly ConcurrentDictionary<Guid, Books> _store = new();
    public Task<IEnumerable<Books>> GetAllAsync(CancellationToken ct, bool asNoTracking = false) => Task.FromResult(_store.Values.AsEnumerable());
    public Task<Books?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.GetValueOrDefault(id));
    public Task<Books?> AddAsync(Books entity, CancellationToken ct) { _store[entity.Id] = entity; return Task.FromResult<Books?>(entity); }
    public Task<bool> UpdateAsync(Books entity, CancellationToken ct) { _store[entity.Id] = entity; return Task.FromResult(true); }
    public Task<bool> DeleteAsync(Books entity, CancellationToken ct) => DeleteAsync(entity.Id, ct);
    public Task<bool> DeleteAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.TryRemove(id, out _));
    public Task<IEnumerable<Books>> GetByAuthorIdAsync(Guid authorId, CancellationToken ct) => Task.FromResult(_store.Values.Where(b => b.Author.Id == authorId));
    public Task<IEnumerable<Books>> GetByNameAsync(string name, CancellationToken ct) => Task.FromResult(_store.Values.Where(b => b.NameBook.Value.Contains(name, StringComparison.OrdinalIgnoreCase)));
}

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _store = new();
    public Task<IEnumerable<User>> GetAllAsync(CancellationToken ct, bool asNoTracking = false) => Task.FromResult(_store.Values.AsEnumerable());
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.GetValueOrDefault(id));
    public Task<User?> AddAsync(User entity, CancellationToken ct) { _store[entity.Id] = entity; return Task.FromResult<User?>(entity); }
    public Task<bool> UpdateAsync(User entity, CancellationToken ct) { _store[entity.Id] = entity; return Task.FromResult(true); }
    public Task<bool> DeleteAsync(User entity, CancellationToken ct) => DeleteAsync(entity.Id, ct);
    public Task<bool> DeleteAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.TryRemove(id, out _));
    public Task<User?> GetByUsernameAsync(string username, CancellationToken ct) => Task.FromResult(_store.Values.FirstOrDefault(u => u.UserName.Value == username));
}

public class InMemoryReviewRepository : IReviewBookRepository
{
    private readonly ConcurrentDictionary<Guid, ReviewBook> _store = new();
    public Task<IEnumerable<ReviewBook>> GetAllAsync(CancellationToken ct, bool asNoTracking = false) => Task.FromResult(_store.Values.AsEnumerable());
    public Task<ReviewBook?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.GetValueOrDefault(id));
    public Task<ReviewBook?> AddAsync(ReviewBook entity, CancellationToken ct) { _store[entity.Id] = entity; return Task.FromResult<ReviewBook?>(entity); }
    public Task<bool> UpdateAsync(ReviewBook entity, CancellationToken ct) { _store[entity.Id] = entity; return Task.FromResult(true); }
    public Task<bool> DeleteAsync(ReviewBook entity, CancellationToken ct) => DeleteAsync(entity.Id, ct);
    public Task<bool> DeleteAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.TryRemove(id, out _));
    public Task<IEnumerable<ReviewBook>> GetByBookIdAsync(Guid bookId, CancellationToken ct) => Task.FromResult(_store.Values.Where(r => r.Book.Id == bookId));
    public Task<IEnumerable<ReviewBook>> GetByUserIdAsync(Guid userId, CancellationToken ct) => Task.FromResult(_store.Values.Where(r => r.User.Id == userId));
}