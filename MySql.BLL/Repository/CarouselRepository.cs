using DAL;

namespace MySql.BLL.Repository
{
    public class CarouselRepository : Repository<Carousel>
    {
        public CarouselRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}