using System;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    // ReSharper disable once UnusedMember.Global
    public sealed class RubricsFeature : Feature<RubricsState>
    {
        public override string GetName() => nameof(RubricsState);

        protected override RubricsState GetInitialState()
        {
            return new RubricsState(ModelStatus.None, Array.Empty<RubricModel>());
        }
    }

    public sealed class RubricsState
    {
        public ModelStatus Status
        {
            get;
        }

        public RubricModel[] Rubrics
        {
            get;
        }

        public RubricsState(ModelStatus status, RubricModel[] rubrics)
        {
            Status = status;
            Rubrics = rubrics;
        }
    }
}