namespace ProjectIdeasLibrary.Models;
public class CommentModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Text { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public BasicUserModel Creator { get; set; }
    public BasicIdeaModel Idea { get; set; }
}
