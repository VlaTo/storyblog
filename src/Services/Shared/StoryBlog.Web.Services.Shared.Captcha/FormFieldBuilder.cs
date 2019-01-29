using System;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FormFieldBuilder
    {
        private string name;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("", nameof(value));
                }

                name = value;
            }
        }
    }
}