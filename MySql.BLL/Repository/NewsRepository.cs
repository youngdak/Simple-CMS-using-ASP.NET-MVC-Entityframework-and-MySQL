using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace MySql.BLL.Repository
{
    public class NewsRepository : Repository<News>
    {
        private readonly ApplicationDbContext _context;

        public NewsRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public override List<News> Get()
        {
            return _context.News.Include("Author").ToList();
        }

        public override Task<List<News>> GetAsync()
        {
            return _context.News.Include("Author").ToListAsync();
        }
    }
}