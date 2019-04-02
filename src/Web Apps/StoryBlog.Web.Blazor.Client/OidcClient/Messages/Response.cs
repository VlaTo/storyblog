using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.JSInterop;

namespace StoryBlog.Web.Blazor.Client.OidcClient.Messages
{
    /// <summary>
    /// A protocol response
    /// </summary>
    public abstract class Response
    {
        /// <summary>
        /// Gets the raw protocol response.
        /// </summary>
        /// <value>
        /// The raw.
        /// </value>
        public string Raw
        {
            get;
        }

        /// <summary>
        /// Gets the protocol response as JSON.
        /// </summary>
        /// <value>
        /// The json.
        /// </value>
        public IDictionary<string, object> Dictionary
        {
            get;
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether an error occurred.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an error occurred; otherwise, <c>false</c>.
        /// </value>
        public bool IsError => false == String.IsNullOrWhiteSpace(Error);

        /// <summary>
        /// Gets the type of the error.
        /// </summary>
        /// <value>
        /// The type of the error.
        /// </value>
        public ResponseErrorType ErrorType
        {
            get;
        }

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        /// <value>
        /// The HTTP status code.
        /// </value>
        public HttpStatusCode HttpStatusCode
        {
            get;
        }

        /// <summary>
        /// Gets the HTTP error reason.
        /// </summary>
        /// <value>
        /// The HTTP error reason.
        /// </value>
        public string HttpErrorReason
        {
            get;
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error
        {
            get
            {
                if (ResponseErrorType.Http == ErrorType)
                {
                    return HttpErrorReason;
                }

                if (ResponseErrorType.Exception == ErrorType)
                {
                    return Exception.Message;
                }

                return TryGet(OidcConstants.TokenResponse.Error);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="raw">The raw response data.</param>
        protected Response(string raw)
        {
            Raw = raw;

            try
            {
                Dictionary = Json.Deserialize<IDictionary<string, object>>(raw);
            }
            catch (Exception ex)
            {
                ErrorType = ResponseErrorType.Exception;
                Exception = ex;

                return;
            }

            if (String.IsNullOrWhiteSpace(Error))
            {
                HttpStatusCode = HttpStatusCode.OK;
                ErrorType = ResponseErrorType.None;
            }
            else
            {
                HttpStatusCode = HttpStatusCode.BadRequest;
                ErrorType = ResponseErrorType.Protocol;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class with an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        protected Response(Exception exception)
        {
            Exception = exception;
            ErrorType = ResponseErrorType.Exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class with an HTTP status code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="content">The response body</param>
        protected Response(HttpStatusCode statusCode, string reason, string content = null)
        {
            HttpStatusCode = statusCode;
            HttpErrorReason = reason;

            if (HttpStatusCode.OK != statusCode)
            {
                ErrorType = ResponseErrorType.Http;
            }

            if (null != content)
            {
                Raw = content;

                try
                {
                    Dictionary = Json.Deserialize<IDictionary<string, object>>(content);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Tries to get a specific value from the JSON response.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        //public string TryGet(string name) => Json.TryGetString(name);
        public string TryGet(string name)
        {
            if (Dictionary.TryGetValue(name, out var value))
            {
                return value.ToString();
            }

            return null;
        }
    }
}