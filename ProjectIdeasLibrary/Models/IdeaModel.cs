namespace ProjectIdeasLibrary.Models;

public class IdeaModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string DescShort { get; set; }
    public string DescLong { get; set; }
    public List<string> Tags { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public StatusModel Status { get; set; }
    public HashSet<string> Stars { get; set; } = new();
    public List<BasicCommentModel> Comments { get; set; } = new();
    public BasicUserModel Owner { get; set; }
    public bool Active { get; set; } = true;
}
