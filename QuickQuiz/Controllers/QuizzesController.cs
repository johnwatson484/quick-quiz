using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuickQuiz.DAL;
using QuickQuiz.Models;
using QuickQuiz.Services;

namespace QuickQuiz.Controllers
{
    public class QuizzesController : Controller
    {
        private Context db;
        private MarkdownService markdownService;

        public QuizzesController()
        {
            db = new Context();
            markdownService = new MarkdownService();
        }

        public QuizzesController(Context db)
        {
            this.db = db;
            markdownService = new MarkdownService();
        }

        // GET: Quizzes
        public ActionResult Index()
        {
            List<Quiz> quizzes;

            if(User.IsInRole("Admin"))
            {
                quizzes = db.Quizzes.Include(x=>x.Questions).OrderBy(x => x.Title).ToList();
            }
            else
            {
                quizzes = db.Quizzes.Include(x => x.Questions).Where(x => x.Active).OrderBy(x => x.Title).ToList();
            }
            return View(quizzes);
        }

        // GET: Quizzes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quizzes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "QuizId,Title,Description,Active")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Quizzes.Add(quiz);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(quiz);
        }

        // GET: Quizzes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = await db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: Quizzes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "QuizId,Title,Description,Active")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(quiz);
        }

        // GET: Quizzes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = await db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Quiz quiz = await db.Quizzes.FindAsync(id);

            foreach (var question in quiz.Questions)
            {
                if (question.MarkdownReference != null)
                {
                    markdownService.Delete(question.MarkdownReference.Value);
                }
            }

            db.Quizzes.Remove(quiz);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
