using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class BaseEntityController : Controller
    {
        //
        // GET: /BaseEntity/
        public IBLL.IBaseEntityService BaseEntityService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

    }
}
