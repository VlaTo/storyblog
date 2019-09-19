namespace StoryBlog.Web.Services.Shared.MarkDown.Elements
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MarkDownTextElement : MarkDownElement
    {
        public string Text
        {
            get;
        }

        public MarkDownTextElement(string text)
        {
            Text = text;
        }
    }
}