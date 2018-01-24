using DAL;

namespace MySql.BLL.Repository
{
    public class EventRepository : Repository<Event>
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}