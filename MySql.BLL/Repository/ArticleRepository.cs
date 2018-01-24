using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace MySql.BLL.Repository
{
    public class ArticleRepository : Repository<Articles>
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public override List<Articles> Get()
        {
            return _context.Articles.Include("Author").ToList();
        }

        public override Task<List<Articles>> GetAsync()
        {
            return _context.Articles.Include("Author").ToListAsync();
        }
    }
}