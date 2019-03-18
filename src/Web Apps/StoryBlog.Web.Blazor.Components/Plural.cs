using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;

namespace StoryBlog.Web.Blazor.Components
{
    public class Plural : BlazorComponent
    {
        private string content;
        private int _value;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        protected int Value
        {
            get => _value;
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;

                UpdateContent(value, true);
            }
        }

        protected override void OnInit()
        {
            base.OnInit();

            UpdateContent(_value, false);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, "span");
            //builder.AddAttribute(1, "datetime", DateTime.Now.ToString("u"));
            builder.AddContent(1, content);
            builder.CloseElement();
        }

        private void UpdateContent(int value, bool invalidateContent)
        {
            content = Pluralize(value);

            if (invalidateContent)
            {
                StateHasChanged();
            }
        }

        private string Pluralize(int value)
        {
            if (value % 1 == 1)
            {
                return "Комментарий";
            }

            var remainder = value % 5;

            if (2 == remainder || 3 == remainder || 4 == remainder)
            {
                return "Комментария";
            }

            return "Комментариев";
        }
    }
}