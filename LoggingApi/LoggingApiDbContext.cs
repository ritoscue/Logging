using Microsoft.EntityFrameworkCore;
using LoggingApi.Models;
namespace LoggingApi
{
    public class LoggingApiDbContext : DbContext
    {
        public LoggingApiDbContext(DbContextOptions options)
        :base(options){

        }

        public DbSet<Log> Logs { get; set; }
    }
}