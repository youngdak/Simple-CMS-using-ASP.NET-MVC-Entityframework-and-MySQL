using DAL;

namespace MySql.BLL.Repository
{
    public class NewHereRepository : Repository<NewHere>
    {
        public NewHereRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}