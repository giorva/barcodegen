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

        public BarcodeGenController(ILogger<BarcodeGenController> logger) => _logger = logger;

        [HttpGet]
        
        public IActionResult Get(String barcodetype, String text)
        {
            //string todo;
            //todo = type + text;
            //return Ok(todo);
            const int DEFAULT_WIDTH = 300;
            const int DEFAULT_HEIGHT = 100;

            BarcodeFormat barcodeFormat;
            

            int height;
            int width;

            barcodeFormat = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), barcodetype);
            
            if (barcodeFormat == BarcodeFormat.QR_CODE) 
            {
                height = DEFAULT_WIDTH;
                width = DEFAULT_WIDTH;
            } else
            {
                height = DEFAULT_HEIGHT;
                width = DEFAULT_WIDTH;                
            }

            var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = barcodeFormat,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    PureBarcode = false
                }
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
            //return Ok(String.Format("{0}", stream.Length));

        }
    }
}