using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Azure.Services.Interaction.B2C.Helper
{
    public class RestResponse<T>
    {
        /// <summary>
        /// Constructs an instance of RestResponse&lt;T&gt; initialized to a
        /// status code and no content object.
        /// </summary>
        /// <param name="statusCode"></param>
        public RestResponse(HttpStatusCode statusCode) : this(statusCode, default)
        {
        }

        /// <summary>
        /// Constructs an instance of RestResponse&lt;T&gt; initialized to a
        /// status code and content object.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="content">The content object.</param>
        public RestResponse(HttpStatusCode statusCode, T content)
        {
            StatusCode = statusCode;
            Content = content;
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the content object, default&lt;T&gt; if not available.
        /// </summary>
        public T Content { get; private set; }

        /// <summary>
        /// Implicitly converts a RestResponse&lt;T&gt; to an ActionResult&lt;T&gt;.
        /// </summary>
        /// <param name="response">The RestResponse&lt;T&gt; instance to convert.</param>
        public static implicit operator ActionResult<T>(RestResponse<T> response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        }
    }
}
