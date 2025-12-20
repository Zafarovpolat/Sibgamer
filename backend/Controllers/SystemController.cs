using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/system")]
    public class SystemController : ControllerBase
    {
        [HttpGet("time")]
        public IActionResult GetServerTime()
        {
            var now = backend.Utils.DateTimeHelper.GetServerLocalDateTimeOffset();
            var result = new
            {
                serverTime = now, 
                timezoneOffsetMinutes = backend.Utils.DateTimeHelper.GetServerTimezoneOffsetMinutes(),
                timezoneId = backend.Utils.DateTimeHelper.GetServerTimezoneId()
            };

            return Ok(result);
        }
    }
}
