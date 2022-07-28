using MongoDB.Driver;

namespace ProjectIdeasLibrary.DataAccess;
public interface IDbConnection
{
    MongoClient Client { get; }
    IMongoCollection<CommentModel> CommentCollection { get; }
    string CommentCollectionName { get; }
    string DbName { get; }
    IMongoCollection<IdeaModel> IdeaCollection { get; }
    string IdeaCollectionName { get; }
    IMongoCollection<StatusModel> StatusCollection { get; }
    string StatusCollectionName { get; }
    IMongoCollection<UserModel> UserCollection { get; }
    string UserCollectionName { get; }
}