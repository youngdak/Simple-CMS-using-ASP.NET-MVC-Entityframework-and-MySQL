using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace MySql.BLL.Repository
{
    public class BatchRepository : Repository<Batch>
    {
        private readonly ApplicationDbContext _context;

        public BatchRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override List<Batch> Get()
        {
            return _context.Batches.Include("Year").ToList();
        }

        public override Task<List<Batch>> GetAsync()
        {
            return _context.Batches.Include("Year").ToListAsync();
        }
    }
}