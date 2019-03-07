namespace StoryBlog.Web.Services.Shared.Captcha
{
    public sealed class FormFieldDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public string FieldName
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string FieldHint
        {
            get;
        }

        public FormFieldDescription(string fieldName, string fieldHint = null)
        {
            FieldName = fieldName;
            FieldHint = fieldHint;
        }
    }
}