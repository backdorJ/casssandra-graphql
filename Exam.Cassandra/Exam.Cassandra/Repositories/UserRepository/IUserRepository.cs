using Exam.Cassandra.Models;

namespace Exam.Cassandra.Repositories.UserRepository;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(User user);
}