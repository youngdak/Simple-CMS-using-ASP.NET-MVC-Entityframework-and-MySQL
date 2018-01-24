using System.Web.Mvc;
using MySql.BLL.Service;

namespace NCCFOhaukwu.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IService _service;

        protected BaseController(IService service)
        {
            _service = service;
        }

        protected IService Service
        {
            get { return _service; }
        }
    }
}