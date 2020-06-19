using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace SmartHomePI.NET.API.Helpers
{
public class PushStreamResult: IActionResult
{
    private readonly Action<Stream> _onStreamAvailabe;
    private readonly string _contentType;

    public PushStreamResult(Action<Stream> onStreamAvailabe, string contentType)
    {
        _onStreamAvailabe = onStreamAvailabe;
        _contentType = contentType;
    }

    public Task ExecuteResultAsync(ActionContext context)
    {
        var stream = context.HttpContext.Response.Body;
        context.HttpContext.Response.GetTypedHeaders().ContentType = new MediaTypeHeaderValue(_contentType);
        _onStreamAvailabe(stream);
        return Task.CompletedTask;
    }
}
}