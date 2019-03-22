using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuickQuiz.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public int QuizId { get; set; }

        [Display(Name = "Question")]
        public string Text { get; set; }

        [Display(Name = "Markdown File")]  

        public Guid? MarkdownReference { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual IList<Answer> Answers { get; set; }

        public Question()
        {
        }

        public Question(int quizId)
        {
            QuizId = quizId;
            Answers = new List<Answer>();
        }

        public string CorrectAnswer
        {
            get
            {
                return Answers == null ? null : Answers.Where(x => x.Correct).Select(x => x.Text).FirstOrDefault();
            }
        }
    }
}