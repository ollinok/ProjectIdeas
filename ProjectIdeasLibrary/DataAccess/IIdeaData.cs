namespace ProjectIdeasLibrary.DataAccess;

public interface IIdeaData
{
    Task CreateIdea(IdeaModel idea);
    Task<List<IdeaModel>> GetAllActiveIdeas();
    Task<List<IdeaModel>> GetAllIdeas();
    Task<IdeaModel> GetIdea(string id);
    Task StarIdea(string ideaId, string userId);
    Task UpdateIdea(IdeaModel idea);
}