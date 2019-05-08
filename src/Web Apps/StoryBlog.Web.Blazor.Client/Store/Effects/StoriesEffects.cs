using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesActionEffect : Effect<GetStoriesAction>
    {
        private readonly IBlogApiClient client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public GetStoriesActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc cref="Effect{TTriggerAction}.HandleAsync" />
        protected override async Task HandleAsync(GetStoriesAction action, IDispatcher dispatcher)
        {
            try
            {
                var result = await client.GetStoriesAsync(action.Includes);

                if (null == result)
                {
                    throw new Exception("No result received");
                }

                var stories = CreateStories(result.Data, result.Meta.Resources.Authors);

                dispatcher.Dispatch(new GetStoriesSuccessAction(stories, result.Meta.Navigation));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoriesFailedAction(exception.Message));
            }
        }

        private static IEnumerable<FeedStory> CreateStories(IEnumerable<StoryModel> source, IEnumerable<AuthorModel> authors)
        {
            var authorIndex = CreateAuthorIndex(authors);
            var stories = new Collection<FeedStory>();

            foreach (var story in source)
            {
                var item = new FeedStory
                {
                    Title = story.Title,
                    Slug = story.Slug,
                    Content = story.Content,
                    Author = authorIndex[story.Author],
                    CommentsCount = story.Comments.Length,
                    Published = DateTimeOffset.Now
                };

                stories.Add(item);
            }

            return stories;
        }

        private static Author[] CreateAuthorIndex(IEnumerable<AuthorModel> authors)
        {
            return authors
                .Select(author => new Author
                {
                    Name = author.Name
                })
                .ToArray();
        }
    }
}
