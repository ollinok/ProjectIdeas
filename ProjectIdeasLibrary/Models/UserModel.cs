namespace ProjectIdeasLibrary.Models;
public class UserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ObjectIdentifier { get; set; }
    public string UserName { get; set; }
    public string MailAddress { get; set; }
    public List<BasicIdeaModel> Ideas { get; set; } = new();
    public List<BasicIdeaModel> StarredIdeas { get; set; } = new();
}
