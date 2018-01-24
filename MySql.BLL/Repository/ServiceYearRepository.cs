using DAL;

namespace MySql.BLL.Repository
{
    public class ServiceYearRepository : Repository<ServiceYear>
    {
        public ServiceYearRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}