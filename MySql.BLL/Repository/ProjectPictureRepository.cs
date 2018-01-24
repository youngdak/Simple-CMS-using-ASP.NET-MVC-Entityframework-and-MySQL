using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace MySql.BLL.Repository
{
    public class ProjectPictureRepository : Repository<ProjectPicture>
    {
        private readonly ApplicationDbContext _context;

        public ProjectPictureRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public override List<ProjectPicture> Get()
        {
            return _context.ProjectPictures.Include("Project").ToList();
        }

        public override Task<List<ProjectPicture>> GetAsync()
        {
            return _context.ProjectPictures.Include("Project").ToListAsync();
        }
    }
}