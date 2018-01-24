using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;

namespace NCCFOhaukwu.Web.Controllers
{
    //[OutputCache(Duration = 3600, VaryByParam = "*")]
    public class ImagesController : BaseController
    {
        public ImagesController(IService service) : base(service)
        {
        }

        public async Task<ActionResult> MiniPic(int? id)
        {
            var corpmemeber = await Service.CorpMember.GetAsync(id);
            return File(corpmemeber.MiniImage, corpmemeber.MiniContentType);
        }

        public async Task<ActionResult> MajorPic(int? id)
        {
            var corpmemeber = await Service.CorpMember.GetAsync(id);
            return File(corpmemeber.MajorImage, corpmemeber.MajorContentType);
        }

        //[Route("Images/albumphoto/{id}/{*name}")]
        public async Task<ActionResult> AlbumPhoto(int? id)
        {
            var albumPhoto = await Service.AlbumPhoto.GetAsync(id);
            return File(albumPhoto.Photo, albumPhoto.ContentType);
        }

        public async Task<ActionResult> ProjectPhoto(int? id)
        {
            var projectPhoto = await Service.ProjectPicture.GetAsync(id);
            return File(projectPhoto.Photo, projectPhoto.ContentType);
        }

        public async Task<ActionResult> NewsPhoto(int? id)
        {
            var newsPhoto = await Service.News.GetAsync(id);
            return File(newsPhoto.Photo, newsPhoto.ContentType);
        }

        public async Task<ActionResult> CarouselImg(int? id)
        {
            var carouselImg = await Service.Carousel.GetAsync(id);
            return File(carouselImg.Img, carouselImg.ContentType);
        }
    }
}