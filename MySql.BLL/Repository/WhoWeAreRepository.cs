using DAL;

namespace MySql.BLL.Repository
{
    public class WhoWeAreRepository : Repository<WhoWeAre>
    {
        public WhoWeAreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}