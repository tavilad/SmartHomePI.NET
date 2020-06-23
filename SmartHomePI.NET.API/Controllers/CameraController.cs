using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
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
        private bool frameAvailable = false;
        private string BOUNDARY = "frame";

        public CameraController()
        {
            cam = MMALCamera.Instance;
            MMALCameraConfig.VideoResolution = new Resolution(800, 600);
            cam.ConfigureCameraSettings();
            //this.ChangeVideoEncodingType().GetAwaiter().GetResult();
        }

        [HttpGet("stream")]
        public IActionResult Stream()
        {
            this.ChangeVideoEncodingType();
            return new PushStreamResult(OnStreamAvailableAsync, "multipart/x-mixed-replace; boundary=frame");
        }

        private void OnStreamAvailableAsync(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            while (true)
            {
                // prepare image data
                byte[] imageData = null;

                if(this.frame == null)
                {
                    continue;
                }

                // this is to make sure memory stream is disposed after using
                using (MemoryStream ms = new MemoryStream())
                {
                    frame.Save(ms, ImageFormat.Jpeg);
                    imageData = ms.ToArray();
                }

                // prepare header
                byte[] header = CreateHeader(imageData.Length);
                // prepare footer
                byte[] footer = CreateFooter();

                // Start writing data
                stream.Write(header, 0, header.Length);
                stream.Write(imageData, 0, imageData.Length);
                stream.Write(footer, 0, footer.Length);
                stream.Flush();
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

                var portConfig = new MMALPortConfig(MMALEncoding.JPEG, MMALEncoding.I420, quality: 90);

                // Create our component pipeline.         
                imgEncoder.ConfigureOutputPort(portConfig, myCaptureHandler);

                cam.Camera.VideoPort.ConnectTo(splitter);
                splitter.Outputs[0].ConnectTo(imgEncoder);
                cam.Camera.PreviewPort.ConnectTo(nullSink);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

                // Process images for 15 seconds.        
                await cam.ProcessAsync(cam.Camera.VideoPort, cts.Token);
            }

        }

        // [HttpGet("stream2")]
        // public HttpResponseMessage GetVideoContent()
        // {
        //     mjpegStream.Start();
        //     var response = Request.CreateResponse();
        //     response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)StartStream);
        //     response.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("multipart/x-mixed-replace; boundary=" + BOUNDARY);
        //     return response;
        // }

        /// <summary>
        /// Craete an appropriate header.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] CreateHeader(int length)
        {
            string header =
                "--" + BOUNDARY + "\r\n" +
                "Content-Type:image/jpeg\r\n" +
                "Content-Length:" + length + "\r\n\r\n";

            return Encoding.ASCII.GetBytes(header);
        }

        public byte[] CreateFooter()
        {
            return Encoding.ASCII.GetBytes("\r\n");
        }

        /// <summary>
        /// Write the given frame to the stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="frame">Bitmap format frame</param>
        private void WriteFrame(Stream stream, Bitmap frame)
        {
            // prepare image data
            byte[] imageData = null;

            // this is to make sure memory stream is disposed after using
            using (MemoryStream ms = new MemoryStream())
            {
                frame.Save(ms, ImageFormat.Jpeg);
                imageData = ms.ToArray();
            }

            // prepare header
            byte[] header = CreateHeader(imageData.Length);
            // prepare footer
            byte[] footer = CreateFooter();

            // Start writing data
            stream.Write(header, 0, header.Length);
            stream.Write(imageData, 0, imageData.Length);
            stream.Write(footer, 0, footer.Length);
        }

        /// <summary>
        /// While the MJPEGStream is running and clients are connected,
        /// continue sending frames.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="httpContent">The content information</param>
        /// <param name="transportContext"></param>
        // private void StartStream(Stream stream, HttpContent httpContent, TransportContext transportContext)
        // {
        //     while (mjpegStream.IsRunning && HttpContext.Current.Response.IsClientConnected)
        //     {
        //         if (frameAvailable)
        //         {
        //             try
        //             {
        //                 WriteFrame(stream, frame);
        //                 frameAvailable = false;
        //             }
        //             catch (Exception e)
        //             {
        //                 System.Diagnostics.Debug.WriteLine(e);
        //             }
        //         }
        //         else
        //         {
        //             Thread.Sleep(30);
        //         }
        //     }
        //     stopStream();
        // }

        // private void stopStream()
        // {
        //     System.Diagnostics.Debug.WriteLine("Stop stream");
        //     mjpegStream.Stop();
        // }

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