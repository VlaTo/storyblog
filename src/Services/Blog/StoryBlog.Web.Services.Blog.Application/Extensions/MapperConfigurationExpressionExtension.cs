using System.Linq;
using AutoMapper;
using StoryBlog.Web.Services.Blog.Application.Models;

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
                .CreateMap<Persistence.Models.Author, Author>()
                .ConstructUsing(author => new Author(author.Id, author.UserName));

            configuration
                .CreateMap<Persistence.Models.Comment, Comment>()
                .ForMember(
                    comment => comment.Id,
                    mapping => mapping.MapFrom(source => source.Id)
                )
                .ForMember(
                    comment => comment.ParentId,
                    mapping => mapping.MapFrom(source => source.ParentId)
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
                .CreateMap<Persistence.Models.Story, Story>()
                .ConstructUsing(story => new Story(story.Id))
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
                    story => story.Created,
                    mapping => mapping.MapFrom(source => source.Created)
                )
                .ForMember(
                    story => story.Modified,
                    mapping => mapping.MapFrom(source => source.Modified)
                )
                .ForMember(
                    story => story.Published,
                    mapping => mapping.MapFrom(source => source.Published)
                )
                .ForMember(
                    story => story.Author,
                    mapping => mapping.MapFrom(source => source.Author)
                )
                .ForMember(
                    story => story.Comments,
                    mapping => mapping.MapFrom(source => source.Comments)
                );

            configuration
                .CreateMap<Persistence.Models.Story, Stories.Models.FeedStory>()
                .ConstructUsing(story => new Stories.Models.FeedStory(story.Id))
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
                /*.ForMember(
                    story => story.IsPublic,
                    mapping => mapping.MapFrom(source => source.IsPublic)
                )*/
                .ForMember(
                    story => story.Created,
                    mapping => mapping.MapFrom(source => source.Created)
                )
                .ForMember(
                    story => story.Modified,
                    mapping => mapping.MapFrom(source => source.Modified)
                )
                /*.ForMember(
                    story => story.Author,
                    mapping => mapping.MapFrom(source => source.Author)
                )*/
                .AfterMap((source, story, context) => story.CommentsCount = source.Comments.Count());

            configuration
                .CreateMap<Persistence.Models.Story, Landing.Models.HeroStory>()
                .ConstructUsing(story => new Landing.Models.HeroStory(story.Id))
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
                /*.ForMember(
                    story => story.IsPublic,
                    mapping => mapping.MapFrom(source => source.IsPublic)
                )*/
                .ForMember(
                    story => story.Created,
                    mapping => mapping.MapFrom(source => source.Created)
                )
                .ForMember(
                    story => story.Modified,
                    mapping => mapping.MapFrom(source => source.Modified)
                )
                /*.ForMember(
                    story => story.Author,
                    mapping => mapping.MapFrom(source => source.Author)
                )*/
                .AfterMap((source, story, context) => story.CommentsCount = source.Comments.Count);

            configuration
                .CreateMap<Persistence.Models.Rubric, Rubric>()
                .ForMember(
                    rubric => rubric.Id,
                    mapping => mapping.MapFrom(source => source.Id)
                )
                .ForMember(
                    rubric => rubric.Name,
                    mapping => mapping.MapFrom(source => source.Name)
                )
                .ForMember(
                    rubric => rubric.Slug,
                    mapping => mapping.MapFrom(source => source.Slug)
                );

            return configuration;
        }
    }
}