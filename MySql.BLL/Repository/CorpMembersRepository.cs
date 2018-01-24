using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace MySql.BLL.Repository
{
    public class CorpMembersRepository : Repository<CorpMember>
    {
        private readonly ApplicationDbContext _context;

        public CorpMembersRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override List<CorpMember> Get()
        {
            return _context.CorpMembers.Include("ServiceYear").Include("Batch").ToList();
        }

        public override Task<List<CorpMember>> GetAsync()
        {
            return _context.CorpMembers.Include("ServiceYear").Include("Batch").ToListAsync();
        }
    }
}