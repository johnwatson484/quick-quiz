using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuickQuiz.DAL;
using QuickQuiz.Models;
using QuickQuiz.Models.ViewModels;
using QuickQuiz.Services;

namespace QuickQuiz.Controllers
{
    public class QuestionsController : Controller
    {
        private Context db;
        private MarkdownService markdownService;

        public QuestionsController()
        {
            db = new Context();
            markdownService = new MarkdownService();
        }

        public QuestionsController(Context db)
        {
            this.db = db;
            markdownService = new MarkdownService();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index(int quizId)
        {
            ViewBag.QuizId = quizId;
            return View(db.Questions.Include(x => x.Quiz).Where(x => x.QuizId == quizId).ToList());
        }

        // GET: Questions/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? questionId)
        {
            if (questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Include(x => x.Answers).Where(x => x.QuestionId == questionId).FirstOrDefault();
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(new QuestionAnswers(question));
        }

        // GET: Questions/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int quizId)
        {
            return View(new QuestionAnswers(quizId));
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(QuestionAnswers questionAnswers, HttpPostedFileBase file)
        {
            if (file != null && Path.GetExtension(file.FileName) != ".md")
            {
                ModelState.AddModelError("MarkdownReference", "Only .md file extensions supported");
            }

            if(questionAnswers.Answers.Where(x=>x.Correct).Count() != 1)
            {
                ModelState.AddModelError("Text", "One correct answer required");
            }

            if (ModelState.IsValid)
            {
                questionAnswers.Question.Answers = questionAnswers.Answers;
                if (file != null)
                {
                    questionAnswers.Question.MarkdownReference = Guid.NewGuid();
                    markdownService.Upload(file, questionAnswers.Question.MarkdownReference.Value);
                }

                db.Questions.Add(questionAnswers.Question);
                db.SaveChanges();
                return RedirectToAction("Index", new { quizId = questionAnswers.Question.QuizId });
            }

            return View(questionAnswers);
        }

        // GET: Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? questionId)
        {
            if (questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Include(x => x.Answers).Where(x => x.QuestionId == questionId).FirstOrDefault();
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(new QuestionAnswers(question));
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionAnswers questionAnswers, HttpPostedFileBase file)
        {
            if (file != null && Path.GetExtension(file.FileName) != ".md")
            {
                ModelState.AddModelError("MarkdownReference", "Only .md file extensions supported");
            }

            if (questionAnswers.Answers.Where(x => x.Correct).Count() != 1)
            {
                ModelState.AddModelError("Text", "One correct answer required");
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (questionAnswers.Question.MarkdownReference == null)
                    {
                        questionAnswers.Question.MarkdownReference = Guid.NewGuid();
                    }
                    markdownService.Upload(file, questionAnswers.Question.MarkdownReference.Value);
                }

                db.Entry(questionAnswers.Question).State = EntityState.Modified;
                foreach(var answer in questionAnswers.Answers)
                {
                    db.Entry(answer).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { quizId = questionAnswers.Question.QuizId });
            }

            return View(questionAnswers);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? questionId)
        {
            if (questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(questionId);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int questionId)
        {
            Question question = db.Questions.Find(questionId);

            if(question.MarkdownReference != null)
            {
                markdownService.Delete(question.MarkdownReference.Value);
            }

            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index", new { quizId = question.QuizId });
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
