using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagerDbContext _context;

        public ITagRepository TagRepository { get; }
        public ITaskTagRepository TaskTagRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public ITaskItemRepository TaskItemRepository { get; }
        public IUserRepository UserRepository { get; }
        public IProjectRepository ProjectRepository { get; }

        public UnitOfWork(
            TaskManagerDbContext context,
            ITagRepository tagRepository,
            ITaskTagRepository taskTagRepository,
            ICommentRepository commentRepository,
            ITaskItemRepository taskItemRepository,
            IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _context = context;
            TagRepository = tagRepository;
            TaskTagRepository = taskTagRepository;
            CommentRepository = commentRepository;
            TaskItemRepository = taskItemRepository;
            UserRepository = userRepository;
            ProjectRepository = projectRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
