namespace ProjectIdeasLibrary.DataAccess;

public interface ICommentData
{
    Task CreateComment(CommentModel comment);
    Task DeleteComment(CommentModel comment);
    Task<List<CommentModel>> GetIdeaComments();
    Task ModifyComment(CommentModel comment);
}