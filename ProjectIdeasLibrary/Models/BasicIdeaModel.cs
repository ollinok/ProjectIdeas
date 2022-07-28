namespace ProjectIdeasLibrary.Models;
public class BasicIdeaModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }

    public BasicIdeaModel() { }

    public BasicIdeaModel(IdeaModel idea)
    {
        Id = idea.Id;
        Title = idea.Title;
    }
}
