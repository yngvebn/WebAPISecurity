using System.Web.Mvc;
using Api.Controllers.ViewModels;
using Api.Infrastructure.RSA;

namespace Api.Controllers
{
    public class HomeController: Controller
    {
         public ActionResult Index()
         {
             IndexViewModel ivm = new IndexViewModel();
             ivm.PrivateRSAKey = RsaGenerator.GenerateKey(true);
             ivm.PublicRSAKey = RsaGenerator.GenerateKey(false);
             return View(ivm);
         }
    }
}