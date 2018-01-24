using DAL;

namespace MySql.BLL.Repository
{
    public class GiveRepository : Repository<Give>
    {
        public GiveRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}