using Microsoft.AspNetCore.Mvc;
using RagWebApi.Models;
using RagWebApi.Service.Documents;

namespace RagWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController(IDocumentService service) : Controller
    {
        private readonly IDocumentService _service = service;
        [HttpPost("[action]")]
        public async Task<JsonResult> BulkImport([FromForm] IFormFile file)
        {
            try
            {
                var res = await _service.BulkInsert(file);
                if (res)
                    return OutputResults.Success("Data has been imported");
                else
                    return OutputResults.Error("Data could not be imported", 400);

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
                _service.StartAnalysis();
                return OutputResults.Success("Analysis started");

            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }

        }

        [HttpPost("[action]")]
        public async Task<JsonResult> SearchDocument([FromBody] SearchDocumentDto model)
        {
            try
            {
                var results = await _service.SearchAsync(model.Query, model.PreviousMessages);
                return OutputResults.Success(results);
            }
            catch (Exception e)
            {
                return OutputResults.Error(e.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetAll()
        {
            try
            {
                var results = await _service.GetAll();
                return OutputResults.Success(results);
            }
            catch (Exception e)
            {
                return OutputResults.Error(e.Message);
            }
        }
    }

    public class SearchDocumentDto
    {

        public List<string> PreviousMessages { get; set; } = [];
        public string Query { get; set; } = "";

    }
}
