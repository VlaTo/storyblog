using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store
{
    // ReSharper disable once UnusedMember.Global
    /*public sealed class RubricsFeature : Feature<RubricsState>
    {
        public override string GetName() => nameof(RubricsState);

        protected override RubricsState GetInitialState()
        {
            return new RubricsState(ModelStatus.None, Array.Empty<RubricModel>());
        }
    }*/

    /*public sealed class RubricsState : IHasModelStatus
    {
        public ModelStatus Status
        {
            get;
        }

        public IEnumerable<RubricModel> Rubrics
        {
            get;
        }

        public RubricsState(ModelStatus status, IEnumerable<RubricModel> rubrics)
        {
            Status = status;
            Rubrics = rubrics;
        }
    }*/
}