using System.Web.Mvc;
using Api.Controllers.ViewModels;
using RsaHelpers;

namespace Api.Controllers
{
    public class HomeController: Controller
    {
         public ActionResult Index()
         {
             IndexViewModel ivm = new IndexViewModel();
             var keys = RsaGenerator.GenerateKeys();
             ivm.PrivateRSAKey = keys.PrivateKey;
             ivm.PublicRSAKey = keys.PublicKey;
             return View(ivm);
         }
    }
}