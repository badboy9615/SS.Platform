using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class DocumentInfoController : Controller
    {
        //
        // GET: /DocumentInfo/
        public IBLL.IDocumentInfoService DocumentInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

    }
}
