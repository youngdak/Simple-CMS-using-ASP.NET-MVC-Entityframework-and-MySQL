using DAL;

namespace MySql.BLL.Repository
{
    public class PortfolioRepository : Repository<Portfolio>
    {
        public PortfolioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}