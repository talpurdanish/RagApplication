using Microsoft.AspNetCore.Mvc;

using RagWebApi.Models;
using RagWebApi.Service.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


namespace RagWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController(IImageService service) : Controller
    {
        private readonly IImageService _imageService = service;
        [HttpPost("[action]")]
        [RequestSizeLimit(104857600)] // 100 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<JsonResult> MultiUpload([FromForm] List<IFormFile> files)
        {
            try
            {
                var results = new List<bool>();
                foreach (var file in files)
                {
                    var r = await _imageService.UploadImageAsync(file.OpenReadStream(), file.FileName, file.ContentType);
                    results.Add(r);
                }

                if (results.All(r => r))
                    return OutputResults.Success("All images uploaded successfully");
                else
                    return OutputResults.Error("Some images failed to upload", 400);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }
        [HttpPost("[action]")]
        public async Task<JsonResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                var result = await _imageService.UploadImageAsync(file.OpenReadStream(), file.FileName, file.ContentType);
                if (result)
                    return OutputResults.Success("Image has been uploaded");
                else
                    return OutputResults.Error("Image failed to upload", 400);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetImage(int id)
        {
            try
            {
                var result = await _imageService.GetImageAsync(id);
                if (result != null)
                    return OutputResults.Success(result);
                else
                    return OutputResults.Error("Image not found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetImageInsight(int id)
        {
            try
            {
                var result = await _imageService.GetAiInsight(id);
                if (result != null)
                    return OutputResults.Success(result);
                else
                    return OutputResults.Error("Image not found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }
        [HttpGet("[action]")]
        public async Task<JsonResult> GetImages()
        {
            try
            {
                var result = await _imageService.GetAllImagesAsync();
                if (result != null)
                    return OutputResults.Success(result);
                else
                    return OutputResults.Error("Image not found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPicture(int id)
        {
            try
            {
                var result = await _imageService.GetImageAsync(id);
                if (result == null)
                    return OutputResults.Error("Image not found", 404);

                byte[] bytes = Convert.FromBase64String(result.Image);

                using MemoryStream inputStream = new(bytes);
                using Image image = Image.Load(inputStream);
                image.Mutate(ctx =>
                {
                    ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max, // crop instead of just shrinking
                        Size = new Size(512, 512),
                    });
                });

                using var ms = new MemoryStream();
                image.SaveAsJpeg(ms, new JpegEncoder { Quality = 75 });


                var type = $"image/{result.Format}";
                return File(ms.ToArray(), type);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> StartAnalysis()
        {
            try
            {
                _imageService.StartAnalysis();
                return OutputResults.Success("Analysis Started");
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }


        [HttpGet("[action]")]
        public async Task<JsonResult> Search([FromQuery] string query)
        {
            try
            {
                var result = await _imageService.SearchAsync(query);
                if (result != null)
                    return OutputResults.Success(result);
                else
                    return OutputResults.Error("No images found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> GenerateImages([FromQuery] string query, string model)
        {
            try
            {
                var result = await _imageService.GenerateImage(query, model);
                if (result == null)
                    return OutputResults.Error("Image not found", 404);
                
                var type = $"image/png";
                return File(result, type);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }
    }
}
