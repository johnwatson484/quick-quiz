using QuickQuiz.DAL;
using QuickQuiz.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace QuickQuiz.Controllers
{
    public class AssessmentController : Controller
    {
        Context db;

        public AssessmentController()
        {
            db = new Context();
        }

        public AssessmentController(Context db)
        {
            this.db = db;
        }

        public ActionResult Start(int quizId, int questions = 50)
        {
            var allQuestions = db.Questions.AsNoTracking().Include(x => x.Quiz).Where(x => x.QuizId == quizId).ToList();
            var assessment = new Assessment(allQuestions.OrderBy(x=> Guid.NewGuid()).Take(questions).ToList());
            Session["Assessment"] = assessment;

            return RedirectToAction("Question");
        }

        public ActionResult Question()
        {
            var assessment = (Assessment)Session["Assessment"];

            if(assessment == null)
            {
                return RedirectToAction("Expired", "Session");
            }

            if (!assessment.IsComplete)
            {
                ViewBag.Title = assessment.Title;
                return View("Question", assessment.GetNext());
            }
            return RedirectToAction("Finish");
        }

        [HttpPost]
        public ActionResult Submit(int questionId, int answerId)
        {
            var assessment = (Assessment)Session["Assessment"];

            if (assessment == null)
            {
                return Json(new { redirect = true });
            }

            var question = assessment.AssessmentQuestions.Where(x => x.Question.QuestionId == questionId).FirstOrDefault();
            var correctAnswerId = question.Question.Answers.Where(x => x.Correct).Select(x => x.AnswerId).FirstOrDefault();

            question.Answered = true;

            if (correctAnswerId == answerId)
            {
                question.Correct = true;
            }

            Session["Assessment"] = assessment;

            return Json(new { correctAnswerId });
        }

        public ActionResult Finish()
        {
            var assessment = (Assessment)Session["Assessment"];

            if (assessment == null)
            {
                return RedirectToAction("Expired", "Session");
            }

            if (!assessment.IsComplete)
            {
                return RedirectToAction("Question");
            }
            return View(assessment);
        }

        public ActionResult Expired()
        {
            return RedirectToAction("Expired", "Session");
        }
    }
}