using Microsoft.AspNetCore.Mvc;

namespace Vagas.Recrutamento.Teste.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}