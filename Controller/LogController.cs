using assignment_3_movievault.DTO;
using assignment_3_movievault.Services;
using Microsoft.AspNetCore.Mvc;

namespace assignment_3_movievault.Controller
{
    [ApiController]
    [Route("api/logs")]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RequestLogDto>>> GetLogs(
            [FromQuery] string? username,
            [FromQuery] DateTime? date,
            [FromQuery] string? requestType,
            [FromQuery] string? route)
        {
            var logs = await _logService.GetLogsAsync(
                username, date, requestType, route);

            return Ok(logs);
        }
    }
}