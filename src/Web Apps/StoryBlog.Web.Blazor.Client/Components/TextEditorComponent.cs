using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StoryBlog.Web.Client.Core;

namespace StoryBlog.Web.Client.Components
{
    public class TextEditorComponent : BootstrapComponentBase
    {
        private static readonly ClassBuilder<TextEditorComponent> classNameBuilder;

        private Timer _timer;
        private bool hasRendered;
        protected ElementReference InnerFrame;

        [Inject]
        public IJSRuntime JsRuntime
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
        public bool ReadOnly
        {
            get; 
            set;
        }

        protected string ClassString
        {
            get;
            private set;
        }

        public TextEditorComponent()
        {
        }

        static TextEditorComponent()
        {
            classNameBuilder = new ClassBuilder<TextEditorComponent>(String.Empty)
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

        protected void DoCommit()
        {
            //var text = "Lorem Ipsum dolor sit amet";
            //await JsRuntime.InvokeVoidAsync("setContent", "__inner_frame_editor_", text);
        }

        protected override void OnInitialized()
        {
            ClassString = classNameBuilder.Build(this, Class);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _timer = new Timer(OnTimerCallback, null, TimeSpan.FromMilliseconds(100), Period.Never);
            }
        }

        protected override void OnDispose()
        {
            _timer?.Dispose();
        }

        private async void OnTimerCallback(object _)
        {
            hasRendered = true;

            _timer.Dispose();
            _timer = null;

            Debug.WriteLine($"Setting edit content: \'{Text}\'");
            await UpdateEditorContentAsync();
            //await JsRuntime.InvokeVoidAsync("setContent", "__inner_frame_editor_", Text);
        }

        private async Task UpdateEditorContentAsync()
        {
            await JsRuntime.InvokeVoidAsync("setContent", "__inner_frame_editor_", Text);
        }
    }
}