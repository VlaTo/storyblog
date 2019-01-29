using System;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    /// <summary>
    /// 
    /// </summary>
    public class CsvParsingException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public CsvParsingException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CsvParsingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}