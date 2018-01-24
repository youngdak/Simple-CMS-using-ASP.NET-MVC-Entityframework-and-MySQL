using DAL;

namespace MySql.BLL.Repository
{
    public class SermonRepository : Repository<Sermon>
    {
        public SermonRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}