using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.SignalR
{
    internal static class ResponseExtensions
    {
        public static Task Write(this HttpResponse response, ArraySegment<byte> data)
        {
            return response.Body.WriteAsync(data.Array, data.Offset, data.Count, response.HttpContext.RequestAborted);
        }

        public static Task Flush(this HttpResponse response)
        {
            return response.Body.FlushAsync(response.HttpContext.RequestAborted);
        }

        /// <summary>
        /// Set the entire response body to <paramref name="data"/>.
        /// This method does not support writing to a response that has already
        /// been written to since it sets <see cref="HttpResponse.ContentLength"/>.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task End(this HttpResponse response, string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            response.ContentLength = bytes.Length;
            await response.Body.WriteAsync(bytes, 0, bytes.Length, response.HttpContext.RequestAborted);
            await response.Body.FlushAsync();
        }
    }
}