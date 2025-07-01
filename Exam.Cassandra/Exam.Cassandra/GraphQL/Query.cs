using Exam.Cassandra.Models;
using Exam.Cassandra.Services.UserService;

namespace Exam.Cassandra.GraphQL;

public class Query
{
    public Task<IEnumerable<User>> GetUsers([Service] IUserService userService) =>
        userService.GetUsersAsync();
}