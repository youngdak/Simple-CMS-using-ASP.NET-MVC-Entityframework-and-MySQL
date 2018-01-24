using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class ProjectController : BaseController
    {
        public ProjectController(IService service) : base(service)
        {
        }

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        [Route("projects/{*name}")]
        public async Task<ActionResult> Project(string name)
        {
            var projects = await Service.Project.GetAsync();
            var project =
                projects.FirstOrDefault(
                    x => Regex.Replace(x.Name, "[^a-zA-Z0-9 ]", "").ToLower() == name.Replace("-", " ").ToLower());

            ViewBag.ProjectName = project.Name;
            return View(project);
        }

        [OutputCache(Duration = 3600, VaryByParam = "*")]
        [Route("projects/{name}/pictures")]
        public async Task<ActionResult> ProjectPictures(string name, int? page)
        {
            var projects = await Service.Project.GetAsync();
            var project =
                projects.FirstOrDefault(
                    x => Regex.Replace(x.Name, "[^a-zA-Z0-9 ]", "").ToLower() == name.Replace("-", " ").ToLower());
            if (project == null)
            {
                return View();
            }
            var projectpictures = await Service.ProjectPicture.GetAsync();

            ViewBag.ProjectName = project.Name;
            return View(projectpictures.Where(x => x.ProjectId == project.Id).ToList().ToPagedList(page ?? 1, 10));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Project.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}