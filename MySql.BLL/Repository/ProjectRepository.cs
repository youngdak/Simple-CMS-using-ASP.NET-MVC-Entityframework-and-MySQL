using DAL;

namespace MySql.BLL.Repository
{
    public class ProjectRepository : Repository<Project>
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}