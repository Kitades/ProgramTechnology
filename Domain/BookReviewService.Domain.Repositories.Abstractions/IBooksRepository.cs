using BookReviewService.Domain.Repositories.Abstractions.Base;

namespace BookReviewService.Domain.Repositories.Abstractions;

public interface IBooksRepository : IRepository<Books, Guid>
{
    Task<IEnumerable<Books>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken);
    Task<IEnumerable<Books>> GetByNameAsync(string name, CancellationToken cancellationToken);
}