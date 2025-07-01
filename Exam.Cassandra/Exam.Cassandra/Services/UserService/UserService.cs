using Exam.Cassandra.Models;
using Exam.Cassandra.Repositories.UserRepository;
using Exam.Cassandra.Requests.AddUserRequest;
using Exam.Cassandra.Requests.UpdateUserRequest;

namespace Exam.Cassandra.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync() => await _repo.GetAllAsync();

    public async Task<Guid> AddUserAsync(AddUserRequest request)
    {
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            UserName = request.UserName,
            Email = request.Email,
            Age = request.Age,
        };
        
        await _repo.AddAsync(user);
        
        return id;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        await _repo.DeleteAsync(id);
        return true;
    }

    public async Task<bool> UpdateUserAsync(UpdateUserRequest request)
    {        
        var user = new User
        {
            Id = request.Id,
            UserName = request.UserName,
            Email = request.Email,
            Age = request.Age
        };
        
        await _repo.UpdateAsync(user);
        return true;
    }
}