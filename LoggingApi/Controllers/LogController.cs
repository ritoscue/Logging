using System;
using System.Linq;
using System.Threading.Tasks;
using LoggingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LoggingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly LoggingApiDbContext _dbContext;

        public LogController(LoggingApiDbContext dbContext){
            _dbContext = dbContext;
        }

        //POST api/[controller]/
        //[Route("Log")]
        [HttpPost]
        public async Task<IActionResult> postLog([FromBody]Log log)
        {
            var _log = new Log
            {
                Title = log.Title,
                Message = log.Message,
                Level = log.Level,
                dtCreation =log.dtCreation,
                UserId = log.UserId
            };
            _dbContext.Logs.Add(_log);
            

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogById), new { id = _log.Id }, null);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetLogById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var log = await _dbContext.Logs.SingleOrDefaultAsync(l => l.Id == id);
            if (log != null)
            {
                return Ok(log);
            }

            return NotFound();
        }

        //GET api/[controller]/User/1234/GetReport[?dtfrom=2012-12-31T22:00:00.000Z&dtTo=2012-12-31T22:00:00.000Z]
        [HttpGet]
        [Route("{User:int}/[action]")]
        public async Task<IActionResult> GetReport(string User, [FromQuery]DateTime dtFrom,[FromQuery]DateTime dtTo){
            // var logs = await _dbContext.Logs.SingleOrDefaultAsync(l => l.UserId == User);
            // var totalLogs = await _dbContext.Logs
            //     .Where(c => c.Name.StartsWith(name))
            //     .LongCountAsync();
            var totalItems = await _dbContext.Logs
                .Where(l => l.UserId == User)
                .LongCountAsync();
            return Ok(totalItems);

        }

    }
}