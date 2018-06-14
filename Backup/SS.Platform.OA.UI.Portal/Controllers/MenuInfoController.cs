using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class MenuInfoController : Controller
    {
        //
        // GET: /MenuInfo/
        public IBLL.IMenuInfoService MenuInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

    }
}
