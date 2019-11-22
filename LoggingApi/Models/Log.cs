using System;

namespace LoggingApi.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
        public string Message { get; set; } 
        public string UserId { get; set; }  
        public DateTime dtCreation { get; set; }

    }
}