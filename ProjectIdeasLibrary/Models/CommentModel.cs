namespace ProjectIdeasLibrary.Models;
public class CommentModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Text { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public string Owner { get; set; }
}
