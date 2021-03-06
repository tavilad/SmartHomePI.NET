using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace SmartHomePI.NET.API.Helpers
{
    public class PushStreamResult : IActionResult
    {
        private readonly Action<Stream,CancellationToken> _onStreamAvailabe;
        private readonly string _contentType;

        private readonly CancellationToken token;

        public PushStreamResult(Action<Stream,CancellationToken> onStreamAvailabe, CancellationToken requestAborted, string contentType)
        {
            _onStreamAvailabe = onStreamAvailabe;
            _contentType = contentType;
            this.token = requestAborted;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var stream = context.HttpContext.Response.Body;
            context.HttpContext.Response.Headers.Add("Age", "0");
            context.HttpContext.Response.Headers.Add("Cache-Control", "no-cache, private");
            context.HttpContext.Response.Headers.Add("Pragma", "no-cache");
            context.HttpContext.Response.Headers.Add("ContentType", _contentType);
            _onStreamAvailabe(stream, token);
            return Task.CompletedTask;
        }
    }
}