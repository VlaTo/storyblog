using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;

namespace StoryBlog.Web.Blazor.Components
{
    public class Plural : BlazorComponent
    {
        private string content;
        private int currentValue;
        private string rules;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        protected int Value
        {
            get => currentValue;
            set
            {
                if (currentValue == value)
                {
                    return;
                }

                currentValue = value;

                UpdateContent(currentValue, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        protected string Rules
        {
            get => rules;
            set
            {
                if (rules == value)
                {
                    return;
                }

                rules = value;

                ParseRules();
                UpdateContent(currentValue, true);
            }
        }

        protected override void OnInit()
        {
            base.OnInit();

            ParseRules();
            UpdateContent(currentValue, false);
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

        private void ParseRules()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private abstract class ValueMatchBase
        {
            protected ValueMatchBase()
            {
            }

            public abstract bool Match(int value);
        }

        /// <summary>
        /// 
        /// </summary>
        private class ExactValueMatch : ValueMatchBase
        {
            public int Value
            {
                get;
            }

            public ExactValueMatch(int value)
            {
                Value = value;
            }

            public override bool Match(int value) => Value == value;
        }

        /// <summary>
        /// 
        /// </summary>
        private class ReminderValueMatch : ValueMatchBase
        {
            public int Remainer
            {
                get;
            }

            public ReminderValueMatch(int remainer)
            {
                Remainer = remainer;
            }

            public override bool Match(int value) => (value % 10) == Remainer;
        }
    }
}