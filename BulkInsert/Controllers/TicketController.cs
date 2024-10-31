using Microsoft.AspNetCore.Mvc;

namespace BulkInsert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        public static List<Ticket> tickets = new List<Ticket>();

        private readonly DataContext _dbContext;

        public TicketController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetTickets()
        {
            Random random = new Random();
            int number = random.Next(1, 20);
            var ticketNumber = 0;
            var userID = 0;

            if (_dbContext.Tickets.Any())
            {
                ticketNumber = await _dbContext.Tickets.MaxAsync(x => x.TicketNumber);
                userID = await _dbContext.Tickets.MaxAsync(p => p.UserID);
            }


            for (int i = 0; i < number; i++)
            {
                Ticket ticket = new Ticket();
                ticket.UserID = userID + 1;
                ticket.TicketNumber = ticketNumber + 1;
                ticketNumber = ticket.TicketNumber;

                tickets.Add(ticket);
            }

            var bulkCopy = new DatabaseOperations().InsertBulkRecord<Ticket>(tickets);
            tickets.Clear();


            return Ok(await _dbContext.Tickets.ToListAsync());
        }


    }
}
