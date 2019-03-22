using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuickQuiz.Controllers
{
    public class SessionController : Controller
    {
        // GET: Session
        [Route("Expired")]
        public ActionResult Expired()
        {
            return View();
        }
    }
}