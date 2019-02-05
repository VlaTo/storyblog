using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Application.Stories.Handlers;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;

namespace StoryBlog.Web.Services.Blog.Application.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AppBlogApplicationDependencies(this IServiceCollection services)
        {
            services
                .AddScoped<IRequest<IPagedQueryResult<Story>>, GetStoriesListQuery>()
                .AddScoped<IRequest<IRequestResult<Story>>, CreateStoryCommand>()
                .AddScoped<IRequest<IRequestResult<Story>>, GetStoryQuery>()
                .AddScoped<IRequestHandler<GetStoriesListQuery, IPagedQueryResult<Story>>, GetStoriesListQueryHandler>()
                .AddScoped<IRequestHandler<CreateStoryCommand, IRequestResult<Story>>, CreateStoryCommandHandler>()
                .AddScoped<IRequestHandler<EditStoryCommand, IRequestResult<Story>>, EditStoryCommandHandler>()
                .AddScoped<IRequestHandler<GetStoryQuery, IRequestResult<Story>>, GetStoryQueryHandler>()
                .AddScoped<IRequestHandler<DeleteStoryCommand, IRequestResult>, DeleteStoryCommandHandler>();

            services.AddAutoMapper(config => {
                config
                    .CreateMap<Persistence.Models.Author, Author>()
                    .ConstructUsing(author => new Author(author.Id, author.UserName));

                config
                    .CreateMap<Persistence.Models.Comment, Comment>()
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

                config
                    .CreateMap<Persistence.Models.Story, Story>()
                    .ForMember(
                        story => story.Id,
                        mapping => mapping.MapFrom(source => source.Id)
                    )
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
                    .ForMember(
                        story => story.Comments,
                        mapping =>
                        {
                            mapping.MapFrom(source => source.Comments);
                        });
            });

            return services;
        }
    }
}
