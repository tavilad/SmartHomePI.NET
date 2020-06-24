using System;
using IronPython.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Scripting.Hosting;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        private ScriptEngine engine;
        private ScriptScope scope;
        private ScriptSource source;
        public CameraController()
        {
            this.engine = Python.CreateEngine();
            this.scope = engine.CreateScope();
            this.source = engine.CreateScriptSourceFromFile("stream.py");
        }

        [HttpGet("startstream")]
        public IActionResult StartStream()
        {
            var test = this.scope.GetVariable<Func<object>>("start_stream");
            test();
            return Ok();
        }
    }
}