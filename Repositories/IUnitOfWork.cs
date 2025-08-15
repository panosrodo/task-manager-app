namespace TaskManagerApp.Repositories
{
    public interface IUnitOfWork
    {
        ITagRepository TagRepository { get; }
        ITaskTagRepository TaskTagRepository { get; }
        ICommentRepository CommentRepository { get; }
        ITaskItemRepository TaskItemRepository { get; }
        IUserRepository UserRepository { get; }
        IProjectRepository ProjectRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
