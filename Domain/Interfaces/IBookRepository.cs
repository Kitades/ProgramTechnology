namespace Booksworm.Domain.Interfaces;

public interface IBookRepository : IRepository<Book, Guid>
{
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(Guid authorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetBooksByTitleAsync(string title, CancellationToken cancellationToken = default);
}