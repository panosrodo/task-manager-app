namespace TaskManagerApp.Services
{
    public interface IApplicationService
    {
        IUserService UserService { get; }
        IProjectService ProjectService { get; }
        ITaskItemService TaskItemService { get; }
        ITagService TagService { get; }
        ITaskTagService TaskTagService { get; }
        ICommentService CommentService { get; }
    }
}