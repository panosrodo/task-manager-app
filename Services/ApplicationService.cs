using AutoMapper;
using TaskManagerApp.Data;
using TaskManagerApp.Repositories;
using Microsoft.Extensions.Logging;

namespace TaskManagerApp.Services
{
    public class ApplicationService : IApplicationService
    {
        public IUserService UserService { get; }
        public IProjectService ProjectService { get; }
        public ITaskItemService TaskItemService { get; }
        public ITagService TagService { get; }
        public ITaskTagService TaskTagService { get; }
        public ICommentService CommentService { get; }

        public ApplicationService(
            TaskManagerDbContext dbContext,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerFactory loggerFactory)
        {
            UserService = new UserService(
                dbContext,
                unitOfWork,
                mapper,
                loggerFactory.CreateLogger<UserService>()
            );
            ProjectService = new ProjectService(
                dbContext,
                mapper,
                loggerFactory.CreateLogger<ProjectService>()
            );
            TaskItemService = new TaskItemService(
                dbContext,
                mapper,
                loggerFactory.CreateLogger<TaskItemService>()
            );
            TagService = new TagService(
                dbContext,
                mapper,
                loggerFactory.CreateLogger<TagService>()
            );
            TaskTagService = new TaskTagService(
                dbContext,
                mapper,
                loggerFactory.CreateLogger<TaskTagService>()
            );
            CommentService = new CommentService(
                dbContext,
                mapper,
                loggerFactory.CreateLogger<CommentService>()
            );
        }
    }
}