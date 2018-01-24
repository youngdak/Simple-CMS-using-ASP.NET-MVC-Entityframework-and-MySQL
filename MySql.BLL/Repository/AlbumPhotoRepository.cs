using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace MySql.BLL.Repository
{
    public class AlbumPhotoRepository : Repository<AlbumPhoto>
    {
        private readonly ApplicationDbContext _context;

        public AlbumPhotoRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public override List<AlbumPhoto> Get()
        {
            return _context.AlbumPhotos.Include("Album").Include("Year").Include("Batch").ToList();
        }

        public override Task<List<AlbumPhoto>> GetAsync()
        {
            return _context.AlbumPhotos.Include("Album").Include("Year").Include("Batch").ToListAsync();
        }
    }
}