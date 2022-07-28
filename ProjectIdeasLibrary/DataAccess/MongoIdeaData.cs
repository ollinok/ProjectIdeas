using Microsoft.Extensions.Caching.Memory;

namespace ProjectIdeasLibrary.DataAccess;
public class MongoIdeaData : IIdeaData
{
    private readonly IDbConnection _db;
    private readonly IUserData _userData;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<IdeaModel> _ideas;
    private const string CacheName = "IdeaData";

    public MongoIdeaData(IDbConnection db, IUserData userData, IMemoryCache cache)
    {
        _db = db;
        _userData = userData;
        _cache = cache;
        _ideas = db.IdeaCollection;
    }

    public async Task<List<IdeaModel>> GetAllIdeas()
    {
        var output = _cache.Get<List<IdeaModel>>(CacheName);
        if (output == null)
        {
            var results = await _ideas.FindAsync(x => true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
        }
        return output;
    }

    public async Task<List<IdeaModel>> GetAllActiveIdeas()
    {
        var output = _cache.Get<List<IdeaModel>>(CacheName);
        if (output == null)
        {
            var results = await _ideas.FindAsync(i => i.Active == true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
        }
        return output;
    }

    public async Task<IdeaModel> GetIdea(string id)
    {
        var results = await _ideas.FindAsync(i => i.Id == id);
        return results.FirstOrDefault();
    }

    public async Task UpdateIdea(IdeaModel idea)
    {
        await _ideas.ReplaceOneAsync(i => i.Id == idea.Id, idea);
        _cache.Remove(CacheName);
    }

    public async Task StarIdea(string ideaId, string userId)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var ideasInTransaction = db.GetCollection<IdeaModel>(_db.IdeaCollectionName);
            var idea = (await ideasInTransaction.FindAsync(i => i.Id == ideaId)).First();

            bool isStarred = idea.Stars.Add(userId);
            if (!isStarred)
            {
                idea.Stars.Remove(userId);
            }

            await ideasInTransaction.ReplaceOneAsync(i => i.Id == ideaId, idea);

            var usersInTransAction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _userData.GetUser(idea.Owner.Id);

            if (isStarred)
            {
                user.StarredIdeas.Add(new BasicIdeaModel(idea));
            }
            else
            {
                var ideaToRemove = user.StarredIdeas.Where(i => i.Id == ideaId).First();
                user.StarredIdeas.Remove(new BasicIdeaModel(idea));
            }
            await usersInTransAction.ReplaceOneAsync(u => u.Id == userId, user);

            await session.CommitTransactionAsync();

            _cache.Remove(CacheName);
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }

    public async Task CreateIdea(IdeaModel idea)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var ideasInTransaction = db.GetCollection<IdeaModel>(_db.IdeaCollectionName);
            await ideasInTransaction.InsertOneAsync(idea);

            var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _userData.GetUser(idea.Owner.Id);
            user.OwnedIdeas.Add(new BasicIdeaModel(idea));
            await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

            await session.CommitTransactionAsync();

            _cache.Remove(CacheName);
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }
}
