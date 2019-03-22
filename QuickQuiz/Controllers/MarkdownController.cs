using QuickQuiz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuickQuiz.Controllers
{
    public class MarkdownController : Controller
    {
        MarkdownService markdownService;

        public MarkdownController()
        {
            markdownService = new MarkdownService();
        }

        // GET: Markdown
        public ActionResult _Markdown(Guid reference)
        {
            return PartialView((object)markdownService.Get(reference));
        }
    }
}