using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class R_Document_UserController : Controller
    {
        //
        // GET: /R_Document_User/
        public IBLL.IDocumentInfoService DocumentInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

    }
}
