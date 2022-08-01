namespace ProjectIdeasLibrary.Models;
public class BasicCommentModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string IdeaTitle { get; set; }

    public BasicCommentModel() { }

    public BasicCommentModel(CommentModel comment)
    {
        Id = comment.Id;
        IdeaTitle = comment.Idea.Title;
    }
}
