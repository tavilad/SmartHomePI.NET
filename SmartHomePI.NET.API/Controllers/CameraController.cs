using System;
using System.Drawing;
using System.Drawing.Imaging;
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
        bool saveOnce = true;
        public  CameraController()
        {
            cam = MMALCamera.Instance;
            MMALCameraConfig.Resolution = new Resolution(800, 600);
            cam.ConfigureCameraSettings();
            this.ChangeVideoEncodingType().GetAwaiter().GetResult();
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
                    if(this.frame == null)
                    {
                        continue;
                    }
                    this.frame.Save(ms, ImageFormat.Jpeg);
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
                if(saveOnce)
                {
                    this.frame.Save("test", ImageFormat.Jpeg);
                    saveOnce = false;
                }
            }
        }

        public async Task ChangeVideoEncodingType()
        {
            // By default, video resolution is set to 1920x1080 which will probably be too large for your project. Set as appropriate using MMALCameraConfig.VideoResolution
            // The default framerate is set to 30fps. You can see what "modes" the different cameras support by looking:
            // https://github.com/techyian/MMALSharp/wiki/OmniVision-OV5647-Camera-Module
            // https://github.com/techyian/MMALSharp/wiki/Sony-IMX219-Camera-Module            
            MMALCamera cam = MMALCamera.Instance;

            using (var myCaptureHandler = new EmguInMemoryCaptureHandler())
            using (var splitter = new MMALSplitterComponent())
            using (var imgEncoder = new MMALImageEncoder(continuousCapture: true))
            using (var nullSink = new MMALNullSinkComponent())
            {
                cam.ConfigureCameraSettings();

                myCaptureHandler.MyEmguEvent += OnEmguEventCallback;
                
                var portConfig = new MMALPortConfig(MMALEncoding.MJPEG, MMALEncoding.I420, 90);

                // Create our component pipeline.         
                imgEncoder.ConfigureOutputPort(portConfig, myCaptureHandler);
                
                splitter.Outputs[0].ConnectTo(imgEncoder);   
                cam.Camera.VideoPort.ConnectTo(splitter);
                 
                cam.Camera.PreviewPort.ConnectTo(nullSink);
                
                // Camera warm up time
                await Task.Delay(2000);
                        
                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(15000));
                
                // Process images for 15 seconds.        
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