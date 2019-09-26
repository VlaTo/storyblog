using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StoryBlog.Web.Client.Core;
using StoryBlog.Web.Client.Extensions;
using StoryBlog.Web.Client.Services;

namespace StoryBlog.Web.Client.Components
{
    public class TextEditorComponent : BootstrapComponentBase
    {
        private static readonly ClassBuilder<TextEditorComponent> ClassNameBuilder;
        private const string EditorObjectNamePrefix = "contentEditor";

        protected const string HostElementId = "__inner_frame_editor_";

        private ITimeout timeout;
        private bool hasRendered;

        [Inject]
        public IJSRuntime JsRuntime
        {
            get;
            set;
        }

        [Inject]
        public ITimeoutManager TimeoutManager
        {
            get; 
            set;
        }

        [Parameter]
        public string Text
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback<string> OnCommit
        {
            get; 
            set;
        }

        protected string ClassString
        {
            get;
            private set;
        }

        static TextEditorComponent()
        {
            ClassNameBuilder = new ClassBuilder<TextEditorComponent>(String.Empty)
                /*.DefineClass(@class => @class
                    .Modifier("outline", component => component.Outline)
                    .Name(component => EnumHelper.GetClassName(component.Type))
                    .Condition(component => BootstrapButtonTypes.Default != component.Type)
                )*/
                //.DefineClass(@class => @class.Name("block").Condition(component => component.Block))
                //.DefineClass(@class => @class.Name("lg").Condition(component => BootstrapButtonSizes.Large == component.Size))
                //.DefineClass(@class => @class.Name("sm").Condition(component => BootstrapButtonSizes.Small == component.Size))
                //.DefineClass(@class => @class.NoPrefix().Name("active").Condition(component => component.Active))
                ;

        }

        protected override async Task OnParametersSetAsync()
        {
            if (hasRendered)
            {
                await UpdateEditorContentAsync();
            }
        }

        protected override void OnInitialized()
        {
            ClassString = ClassNameBuilder.Build(this, Class);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timeout = TimeoutManager.CreateTimeout(OnTimerCallback, TimeSpan.FromMilliseconds(50));
            }
        }

        protected override void OnDispose()
        {
            timeout?.Dispose();
        }

        protected async void DoCommit()
        {
            var text = await JsRuntime.InvokeAsync<string>(AddEditorPrefix("getContent"), HostElementId);
            await OnCommit.InvokeAsync(text);
        }

        private async void OnTimerCallback()
        {
            hasRendered = true;
            timeout = null;

            await UpdateEditorContentAsync();
        }

        private async Task UpdateEditorContentAsync()
        {
            var content = Text;

            if (String.IsNullOrEmpty(content))
            {
                content = "<p class=\"storyblog-content-p\"></p>";
            }

            await JsRuntime.InvokeVoidAsync(
                AddEditorPrefix("setContent"),
                HostElementId,
                content
            );
        }

        private static string AddEditorPrefix(string funcName)
        {
            if (String.IsNullOrEmpty(EditorObjectNamePrefix))
            {
                return funcName;
            }

            var name = EditorObjectNamePrefix;

            if (false == EditorObjectNamePrefix.EndsWith('.'))
            {
                name += '.';
            }

            return name + funcName;
        }
    }
}