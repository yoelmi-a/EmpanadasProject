using Microsoft.AspNetCore.Mvc;

namespace EmpanadasProject.Web.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
