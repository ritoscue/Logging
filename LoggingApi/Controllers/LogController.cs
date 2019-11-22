using System;
using System.Linq;
using System.Threading.Tasks;
using LoggingApi.Models;
using LoggingApi.ModelView;
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
        public IActionResult GetReport(string User, [FromQuery]DateTime dtFrom,[FromQuery]DateTime dtTo){
            var root = (IQueryable<Log>)_dbContext.Logs;
            root = root.Where(l => l.UserId == User);
            root = root.Where(l => l.dtCreation >= dtFrom && l.dtCreation <= dtTo);

            var totalLogs = root.Count();            
            var countLevel =  root
                           .GroupBy(p => p.Level)
                           .Select(g => new LevelDTO {Level= g.Key, Count = g.Count()});
            ReportDTO report = new ReportDTO(totalLogs, countLevel);
            return Ok(report);

        }

        //GET api/[controller]/GetLog?[?dtfrom=2012-12-31T22:00:00.000Z&dtTo=2012-12-31T22:00:00.000Z&Level=Error&pageSize=3&pageIndex=1]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetLog([FromQuery]DateTime dtFrom,[FromQuery]DateTime dtTo, [FromQuery]string Level, [FromQuery]int pageSize = 3, [FromQuery]int pageIndex = 0){
            var root = (IQueryable<Log>)_dbContext.Logs;

            if (Level != null)
            {
                root = root.Where(l => l.Level ==  Level);
            }           
            root =  root.Where(l => l.dtCreation >= dtFrom && l.dtCreation <= dtTo);

            var totalLogs = await root
                .LongCountAsync();

            var logsOnPage = await root
                .Skip(pageSize * pageIndex)
                .OrderBy(l=>l.UserId)
                .Take(pageSize)
                .ToListAsync();
            

            var model = new PageViewModel<Log>(
                pageIndex, pageSize, totalLogs, logsOnPage);

            return Ok(model);

        }

    }
}