using System.Linq;
using AutoMapper;

namespace StoryBlog.Web.Services.Blog.Application.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MapperConfigurationExpressionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IMapperConfigurationExpression AddBlogApplicationTypeMappings(this IMapperConfigurationExpression configuration)
        {
            configuration
                .CreateMap<Persistence.Models.Author, Stories.Models.Author>()
                .ConstructUsing(author => new Stories.Models.Author(author.Id, author.UserName));

            configuration
                .CreateMap<Persistence.Models.Comment, Stories.Models.Comment>()
                .ForMember(
                    comment => comment.Id,
                    mapping => mapping.MapFrom(source => source.Id)
                )
                .ForMember(
                    comment => comment.Content,
                    mapping => mapping.MapFrom(source => source.Content)
                )
                .ForMember(
                    comment => comment.Author,
                    mapping => mapping.MapFrom(source => source.Author)
                )
                .ForMember(
                    comment => comment.Created,
                    mapping => mapping.MapFrom(source => source.Created)
                )
                .ForMember(
                    comment => comment.Modified,
                    mapping => mapping.MapFrom(source => source.Modified)
                );

            configuration
                .CreateMap<Persistence.Models.Story, Stories.Models.Story>()
                .ConstructUsing(story => new Stories.Models.Story(story.Id))
                .ForMember(
                    story => story.Slug,
                    mapping => mapping.MapFrom(source => source.Slug)
                )
                .ForMember(
                    story => story.Title,
                    mapping => mapping.MapFrom(source => source.Title)
                )
                .ForMember(
                    story => story.Content,
                    mapping => mapping.MapFrom(source => source.Content)
                )
                .ForMember(
                    story => story.IsPublic,
                    mapping => mapping.MapFrom(source => source.IsPublic)
                )
                .ForMember(
                    story => story.Created,
                    mapping => mapping.MapFrom(source => source.Created)
                )
                .ForMember(
                    story => story.Modified,
                    mapping => mapping.MapFrom(source => source.Modified)
                )
                .ForMember(
                    story => story.Author,
                    mapping => mapping.MapFrom(source => source.Author)
                )
                .AfterMap((source, story, context) => story.Comments.AddRange(
                        source.Comments?.Select(comment => context.Mapper.Map<Stories.Models.Comment>(comment))
                    )
                );

            return configuration;
        }
    }
}