using DAL;

namespace MySql.BLL.Repository
{
    public class AlbumRepository : Repository<Album>
    {
        public AlbumRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}