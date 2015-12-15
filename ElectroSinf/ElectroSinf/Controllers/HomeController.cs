using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroSinf.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Response.Redirect("http://localhost:3000/electrosinf/site/pages/homepage.php");
            return null;
        }
    }
}
