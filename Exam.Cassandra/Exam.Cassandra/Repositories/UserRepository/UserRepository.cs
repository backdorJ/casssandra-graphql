using Cassandra;
using Exam.Cassandra.Models;
using ISession = Cassandra.ISession;

namespace Exam.Cassandra.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private const string UserTableName = "users";
    private const string Keyspace = "testkeyspace";
    
    private readonly ISession _session;

    public UserRepository(IConfiguration config)
    {
        var s = config.GetValue<string>("Cassandra:Server");
        var t = config.GetValue<int>("Cassandra:Port");
        var cluster = Cluster.Builder()
            .AddContactPoint(config.GetValue<string>("Cassandra:Server"))
            .WithPort(config.GetValue<int>("Cassandra:Port"))
            .Build();

        _session = cluster.Connect();

        CreateKeyspaceIfNotExistsAsync().Wait();
        CreateTableIfNotExistsAsync().Wait();
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var result = await _session.ExecuteAsync(new SimpleStatement($@"SELECT
                                        {nameof(User.Id)},
                                         {nameof(User.UserName)},
                                          {nameof(User.Email)},
                                           {nameof(User.Age)}
                                FROM {UserTableName}"));

        var users = result.Select(row => new User
        {
            Id = row.GetValue<Guid>(nameof(User.Id).ToLower()),
            UserName = row.GetValue<string>(nameof(User.UserName).ToLower()),
            Email = row.GetValue<string>(nameof(User.Email).ToLower()),
            Age = row.GetValue<int>(nameof(User.Age).ToLower()),
        });

        return users;
    }

    public async Task AddAsync(User user)
    {
        var statement = new SimpleStatement(
            $@"INSERT INTO {UserTableName} 
                        ({nameof(User.Id)}, {nameof(User.UserName)},
                         {nameof(User.Age)},
                          {nameof(User.Email)})
                    VALUES (?, ?, ?, ?)", user.Id,  user.UserName, user.Age, user.Email);
        
        await _session.ExecuteAsync(statement);
    }
    public async Task DeleteAsync(Guid id)
    {
        var statement = new SimpleStatement($"DELETE FROM {UserTableName} WHERE {nameof(User.Id)} = ?", id);
        await _session.ExecuteAsync(statement);
    }

    public async Task UpdateAsync(User user)
    {
        var statement = new SimpleStatement(
            $@"UPDATE {UserTableName}
                SET {nameof(User.UserName)} = ?,
                    {nameof (User.Age)} = ?,
                    {nameof (User.Email)} = ?
                WHERE {nameof(User.Id)} = ?",
            user.UserName, user.Age, user.Email, user.Id);
        
        await _session.ExecuteAsync(statement);
    }
    
    private async Task CreateTableIfNotExistsAsync()
    {
        var query = $@"
            CREATE TABLE IF NOT EXISTS {UserTableName} (
                {nameof(User.Id)} uuid PRIMARY KEY,
                {nameof(User.UserName)} text,
                {nameof(User.Age)} int,
                {nameof(User.Email)} text
            )";

        await _session.ExecuteAsync(new SimpleStatement(query));
    }
    
    private async Task CreateKeyspaceIfNotExistsAsync()
    {
        var query = $@"
            CREATE KEYSPACE IF NOT EXISTS {Keyspace}
            WITH replication = {{ 'class': 'SimpleStrategy', 'replication_factor': '1' }}";

        await _session.ExecuteAsync(new SimpleStatement(query));
        _session.ChangeKeyspace(Keyspace);
    }
}