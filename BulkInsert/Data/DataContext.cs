global using Microsoft.EntityFrameworkCore;

namespace BulkInsert.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }
    }
}
