using Microsoft.AspNetCore.Mvc;

using RagWebApi.Models;
using RagWebApi.Service;
using RagWebApi.Service.Movies;
using RagWebApi.Viewmodels;



namespace RagWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController(IMovieService service) : Controller
    {
        private readonly IMovieService _moviesService = service;
        
        [RequestSizeLimit(104857600)] // 100 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [HttpPost("[action]")]
        public async Task<JsonResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                var result = await _moviesService.UploadMoviesAsync(file.OpenReadStream());
                if (result)
                    return OutputResults.Success("Movie has been uploaded");
                else
                    return OutputResults.Error("Movie failed to upload", 400);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetMovie(int id)
        {
            try
            {
                var result = await _moviesService.GetMovieAsync(id);
                if (result != null)
                    return OutputResults.Success(result);
                else
                    return OutputResults.Error("Movie not found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }
        [HttpGet("[action]")]
        public async Task<JsonResult> GetMovies([FromQuery] DataFilter filter)
        {
            try
            {
                var result = await _moviesService.GetAllMoviesAsync(filter);
                if (result != null)
                    return OutputResults.Success(result);
                else
                    return OutputResults.Error("Movie not found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }

        [HttpGet("[action]/{type}")]
        public async Task<JsonResult> StartAnalysis(int type)
        {
            try
            {
                await _moviesService.StartAnalysis(type);
                return OutputResults.Success("Analysis Started");
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }


        [HttpGet("[action]")]
        public async Task<JsonResult> Search([FromQuery] WeightedQuery weightedQuery)
        {
            try
            {
                var result = await _moviesService.SearchAsync(weightedQuery);
                if (result != null)
                    return OutputResults.Success(result);
                else
                    return OutputResults.Error("No movies found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }


        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> RecommendMovies(int id)
        {
            try
            {
                var result = await _moviesService.RecommendAsync(id);
                if (result != null)
                    return OutputResults.Success(result);
                else
                    return OutputResults.Error("Movie not found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }
    }
}
