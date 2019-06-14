using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Blog.Application.Rubrics.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Blog.Application.Rubrics.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetRubricsQueryHandler : IRequestHandler<GetRubricsQuery, IRequestResult<IReadOnlyCollection<Rubric>>>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GetRubricsQueryHandler> logger;

        public GetRubricsQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetRubricsQueryHandler> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IRequestResult<IReadOnlyCollection<Rubric>>> Handle(GetRubricsQuery request, CancellationToken cancellationToken)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var rubrics = await context.Rubrics
                    .AsNoTracking()
                    .OrderBy(rubric => rubric.Order)
                    //.OrderBy(rubric => rubric.Name, StringComparer.InvariantCultureIgnoreCase)
                    .ThenBy(rubric => rubric.Name)
                    .Select(rubric => mapper.Map<Rubric>(rubric))
                    .ToArrayAsync(cancellationToken: cancellationToken);

                return RequestResult.Success(rubrics);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"{nameof(GetRubricsQueryHandler)}");
                return RequestResult.Success(new ReadOnlyCollection<Rubric>(new List<Rubric>()));
            }
        }
    }
}