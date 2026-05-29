namespace Booksworm.Domain.Interfaces;

public interface IAuthorRepository : IRepository<Author, Guid>
{
    Task<Author?> GetAuthorByNameAsync(string name, CancellationToken cancellationToken = default);
}