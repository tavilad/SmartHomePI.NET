using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHomePI.NET.API.Helpers;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Camera;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        private Bitmap frame = null;
        public CameraController()
        {
            this.TestCaptureVideo();
        }
        [HttpGet]
        public IActionResult Stream(int channel)
        {
            var response = new HttpResponseMessage();
            return new PushStreamResult(OnStreamAvailable, "multipart/x-mixed-replace; boundary=boundary");
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

                    writer.WriteLine("--boundary");
                    writer.WriteLine("Content-Type: image/jpeg");
                    writer.WriteLine(string.Format("Content-length: {0}", buffer.Length));
                    writer.WriteLine();
                    writer.Write(buffer);

                    writer.Flush();
                }
            }
        }
        private void TestCaptureVideo()
        {
            // Setup our working variables
            var videoByteCount = 0;
            var videoEventCount = 0;
            var startTime = DateTime.UtcNow;

            // Configure video settings
            var videoSettings = new CameraVideoSettings()
            {
                CaptureTimeoutMilliseconds = 0,
                CaptureDisplayPreview = false,
                ImageFlipVertically = true,
                CaptureExposure = CameraExposureMode.Night,
                CaptureWidth = 1920,
                CaptureHeight = 1080
            };

            try
            {
                // Start the video recording
                Pi.Camera.OpenVideoStream(videoSettings,
                    onDataCallback: (data) => { 
                        using (var ms = new MemoryStream(data))
                        {
                            this.frame = new Bitmap(ms);
                        } },
                    onExitCallback: null);

                // Wait for user interaction
                startTime = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()}: {ex.Message}");
            }
            finally
            {
                // Always close the video stream to ensure raspivid quits
                Pi.Camera.CloseVideoStream();

                // Output the stats
                var megaBytesReceived = (videoByteCount / (1024f * 1024f)).ToString("0.000");
                var recordedSeconds = DateTime.UtcNow.Subtract(startTime).TotalSeconds.ToString("0.000");
            }            
        }
    }
}