using Exam.Cassandra.Requests.AddUserRequest;
using Exam.Cassandra.Requests.UpdateUserRequest;
using Exam.Cassandra.Services.UserService;

namespace Exam.Cassandra.GraphQL;

public class Mutation
{
    public Task<Guid> AddUser(AddUserRequest request, [Service] IUserService service) =>
        service.AddUserAsync(request);
    
    public Task<bool> DeleteUser(Guid id, [Service] IUserService service) =>
        service.DeleteUserAsync(id);
    
    public Task<bool> UpdateUser(UpdateUserRequest request, [Service] IUserService service) =>
        service.UpdateUserAsync(request);
}