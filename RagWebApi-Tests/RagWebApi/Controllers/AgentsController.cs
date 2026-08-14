using Microsoft.AspNetCore.Mvc;
using RagWebApi.Agent;
using RagWebApi.Models;

namespace RagWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentsController(ISupervisorAgent agent) : Controller
    {

        

        [HttpGet("[action]")]
        public async Task<JsonResult> Run([FromQuery] string input, string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    return OutputResults.Error("Input is required", 404);
                }

               
                var result = await agent.ProcessRequestAsync(input, sessionId);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result.Message);
                Console.ResetColor();
                if (result != null)
                    return OutputResults.Success(result, 200);
                else
                    return OutputResults.Error("Tasks not found", 404);
            }
            catch (Exception ex)
            {
                return OutputResults.Error(ex.Message, 500);
            }
        }
    }
}
