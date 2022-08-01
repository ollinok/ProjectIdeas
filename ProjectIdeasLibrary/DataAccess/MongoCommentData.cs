using Microsoft.Extensions.Caching.Memory;

namespace ProjectIdeasLibrary.DataAccess;
public class MongoCommentData : ICommentData
{
    private readonly IMongoCollection<CommentModel> _comments;
    private readonly IMemoryCache _cache;
    private const string cacheName = "CommentData";

    public MongoCommentData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _comments = db.CommentCollection;
    }

    public async Task<List<CommentModel>> GetIdeaComments(string id)
    {
        //var output = _cache.Get<List<CommentModel>>(cacheName);
        //if (output == null)
        //{
        //    var results = await _comments.FindAsync(x => true);
        //    output = results.ToList();

        //    _cache.Set(cacheName, output, TimeSpan.FromMinutes(1));
        //}

        var output = new List<CommentModel>();
        var results = await _comments.FindAsync(c => c.Idea.Id == id);
        output = results.ToList();

        return output;
    }

    public Task CreateComment(CommentModel comment)
    {
        return _comments.InsertOneAsync(comment);
    }

    public Task ModifyComment(CommentModel comment)
    {
        var filter = Builders<CommentModel>.Filter.Eq("Id", comment.Id);
        return _comments.ReplaceOneAsync(filter, comment, new ReplaceOptions { IsUpsert = true });
    }

    public Task DeleteComment(CommentModel comment)
    {
        var filter = Builders<CommentModel>.Filter.Eq("Id", comment.Id);
        return _comments.DeleteOneAsync(filter);
    }
}
