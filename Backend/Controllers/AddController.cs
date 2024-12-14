using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddController : ControllerBase
    {
        public record AddRequest(int FirstNumber, int SecondNumber);
        public record AddResponse
        {
            public int Result { get; init; }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRequest request)
        {
            if (request is null)
            {
                return BadRequest("Request cannot be null");
            }

            try
            {
                await Task.Delay(2000);
                var result = new AddResponse { Result = request.FirstNumber + request.SecondNumber };
                return Ok(result);
            }
            catch (OverflowException)
            {
                return BadRequest("Overflow exception");
            }
        }
    }
}
