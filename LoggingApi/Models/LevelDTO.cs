using System.Collections.Generic;

namespace LoggingApi.Models
{
    public class ReportDTO{
        public int CountLogs { get; set; }
        public IEnumerable<LevelDTO> ListLevelDTO { get; set; }

        public ReportDTO(int _count, IEnumerable<LevelDTO> _levelGroup){
            CountLogs = _count;
            ListLevelDTO = _levelGroup;
        }
    }
    public class LevelDTO
    {
        public string Level { get; set; }
        public int Count { get; set; }
    }
}