using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ProjectIdeasLibrary.DataAccess;
public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private string _connectionId = "MongoDB";
    public string DbName { get; private set; }
    public string CommentCollectionName { get; private set; } = "comments";
    public string StatusCollectionName { get; private set; } = "statuses";
    public string UserCollectionName { get; private set; } = "users";
    public string IdeaCollectionName { get; private set; } = "ideas";

    public MongoClient Client { get; private set; }
    public IMongoCollection<CommentModel> CommentCollection { get; private set; }
    public IMongoCollection<StatusModel> StatusCollection { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }
    public IMongoCollection<IdeaModel> IdeaCollection { get; private set; }

    public DbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        CommentCollection = _db.GetCollection<CommentModel>(CommentCollectionName);
        StatusCollection = _db.GetCollection<StatusModel>(StatusCollectionName);
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
        IdeaCollection = _db.GetCollection<IdeaModel>(IdeaCollectionName);
    }
}
