using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZXing;
using ZXing.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;


namespace barcodegen.Controllers
{

    [ApiController]
    [Route("[controller]/{barcodetype}/{text}")]
    public class BarcodeGenController : ControllerBase
    {
        private readonly ILogger<BarcodeGenController> _logger;
        private const int DEFAULT_WIDTH = 300;
        private const int DEFAULT_HEIGHT = 300;

        public BarcodeGenController(ILogger<BarcodeGenController> logger) => _logger = logger;

        [HttpGet]
        

        public IActionResult Get(String barcodetype, String text, 
                                    [FromQuery]  EncodingOptions ProvidedOptions)
        {
            if (ProvidedOptions.Width == 0) 
            {
                ProvidedOptions.Width = DEFAULT_WIDTH;
            }

            if (ProvidedOptions.Height == 0) 
            {
                ProvidedOptions.Height = DEFAULT_HEIGHT;
            }

            var barcodeFormat = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), barcodetype);
            
            var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = barcodeFormat,
                Options = ProvidedOptions
            };

            Image<Rgba32> bitmap;

            try
            {
                bitmap = barcodeWriter.Write(text);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            
            Stream stream = new System.IO.MemoryStream();
            bitmap.SaveAsPng(stream);
            stream.Position = 0;
            return File(stream, "image/png");


        }
    }
}