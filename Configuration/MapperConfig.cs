using AutoMapper;
using TaskManagerApp.Data;
using TaskManagerApp.DTO;

namespace TaskManagerApp.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // USER MAPPINGS
            // Map Id (entity) -> UserId (DTO)
            CreateMap<User, UserReadOnlyDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<UserSignUpDTO, User>();

            // PROJECT MAPPINGS
            // Map Id (entity) -> ProjectId (DTO)
            CreateMap<Project, ProjectReadOnlyDTO>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ProjectCreateDTO, Project>();
            CreateMap<ProjectUpdateDTO, Project>();

            // TASK ITEM MAPPINGS
            // Map Id (entity) -> TaskItemId (DTO)
            CreateMap<TaskItem, TaskItemReadOnlyDTO>()
                .ForMember(dest => dest.TaskItemId, opt => opt.MapFrom(src => src.Id));
            CreateMap<TaskItemCreateDTO, TaskItem>();
            CreateMap<TaskItemUpdateDTO, TaskItem>();

            // COMMENT MAPPINGS
            // Map Id (entity) -> CommentId (DTO)
            // Map InsertedAt (entity) -> CreatedAt (DTO)
            CreateMap<Comment, CommentReadOnlyDTO>()
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.InsertedAt));
            CreateMap<CommentCreateDTO, Comment>();
            CreateMap<CommentUpdateDTO, Comment>();

            // TAG MAPPINGS
            // Map Id (entity) -> TagId (DTO)
            CreateMap<Tag, TagReadOnlyDTO>()
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.Id));
            CreateMap<TagCreateDTO, Tag>();
            CreateMap<TagUpdateDTO, Tag>();

            // TASKTAG MAPPINGS
            CreateMap<TaskTag, TaskTagReadOnlyDTO>();
            CreateMap<TaskTagCreateDTO, TaskTag>();
        }
    }
}
