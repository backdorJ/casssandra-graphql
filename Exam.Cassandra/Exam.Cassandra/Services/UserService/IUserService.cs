using Exam.Cassandra.Models;
using Exam.Cassandra.Requests.AddUserRequest;
using Exam.Cassandra.Requests.UpdateUserRequest;

namespace Exam.Cassandra.Services.UserService;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<Guid> AddUserAsync(AddUserRequest request);
    Task<bool> DeleteUserAsync(Guid id);
    Task<bool> UpdateUserAsync(UpdateUserRequest request);
}