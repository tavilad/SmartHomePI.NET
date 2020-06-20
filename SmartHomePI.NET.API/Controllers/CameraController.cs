using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using SmartHomePI.NET.API.Helpers;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        private Bitmap frame = null;
        private MMALCamera cam;
        public CameraController()
        {
            cam = MMALCamera.Instance;
            MMALCameraConfig.Resolution = new Resolution(800, 600);
            cam.ConfigureCameraSettings();
            this.ChangeVideoEncodingType();
        }
        [HttpGet]
        public IActionResult Stream(int channel)
        {
            var response = new HttpResponseMessage();
            return new PushStreamResult(OnStreamAvailable, "multipart/x-mixed-replace; boundary=frame");
        }

        private void OnStreamAvailable(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            while (true)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.SetLength(0);
                    this.frame.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] buffer = ms.GetBuffer();

                    writer.WriteLine("--frame");
                    writer.WriteLine("Content-Type: image/jpeg");
                    writer.WriteLine(string.Format("Content-length: {0}", buffer.Length));
                    writer.WriteLine();
                    writer.Write(buffer);

                    writer.Flush();
                }
            }
        }
        protected virtual void OnEmguEventCallback(object sender, EmguEventArgs args)
        {
            Console.WriteLine("I'm in OnEmguEventCallback.");

            using (var ms = new MemoryStream(args.ImageData))
            {
                this.frame = new Bitmap(ms);
            }
        }

        public async Task ChangeVideoEncodingType()
        {
            using (var vidCaptureHandler = new EmguInMemoryCaptureHandler())
            using (var splitter = new MMALSplitterComponent())
            using (var renderer = new MMALNullSinkComponent())
            {
                cam.ConfigureCameraSettings();
                
                // Register to the event.
                vidCaptureHandler.MyEmguEvent += this.OnEmguEventCallback;

                // We are instructing the splitter to do a format conversion to BGR24.
                var splitterPortConfig = new MMALPortConfig(MMALEncoding.BGR24, MMALEncoding.BGR24, 0, 0, null);

                // By default in MMALSharp, the Video port outputs using proprietary communication (Opaque) with a YUV420 pixel format.
                // Changes to this are done via MMALCameraConfig.VideoEncoding and MMALCameraConfig.VideoSubformat.                
                splitter.ConfigureInputPort(new MMALPortConfig(MMALEncoding.OPAQUE, MMALEncoding.I420), cam.Camera.VideoPort, null);

                // We then use the splitter config object we constructed earlier. We then tell this output port to use our capture handler to record data.
                splitter.ConfigureOutputPort<SplitterVideoPort>(0, splitterPortConfig, vidCaptureHandler);

                cam.Camera.PreviewPort.ConnectTo(renderer);
                cam.Camera.VideoPort.ConnectTo(splitter);

                // Camera warm up time
                await Task.Delay(2000).ConfigureAwait(false);

                // Record for 10 seconds. Increase as required.
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1000));

                await cam.ProcessAsync(cam.Camera.VideoPort, cts.Token);
            }
        }

    public class EmguEventArgs : EventArgs
    {
        public byte[] ImageData { get; set; }
    }

    public class EmguInMemoryCaptureHandler : InMemoryCaptureHandler, IVideoCaptureHandler
    {
        public event EventHandler<EmguEventArgs> MyEmguEvent;

        public override void Process(ImageContext context)
        {
            // The InMemoryCaptureHandler parent class has a property called "WorkingData". 
            // It is your responsibility to look after the clearing of this property.

            // The "eos" parameter indicates whether the MMAL buffer has an EOS parameter, if so, the data that's currently
            // stored in the "WorkingData" property plus the data found in the "data" parameter indicates you have a full image frame.

            // I suspect in here, you will want to have a separate thread which is responsible for sending data to EmguCV for processing?
            Console.WriteLine("I'm in here");

            base.Process(context);

            if (context.Eos)
            {
                this.MyEmguEvent(this, new EmguEventArgs { ImageData = this.WorkingData.ToArray() });

                this.WorkingData.Clear();
                Console.WriteLine("I have a full frame. Clearing working data.");
            }
        }

        public void Split()
        {
            throw new NotImplementedException();
        }
    }
}
}