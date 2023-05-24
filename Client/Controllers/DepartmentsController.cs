using Microsoft.AspNetCore.Mvc;
using Client.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
