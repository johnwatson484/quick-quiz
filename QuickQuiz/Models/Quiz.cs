using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuickQuiz.Models
{
    [Table("Quizzes")]
    public class Quiz
    {
        public int QuizId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; } = true;

        public virtual IList<Question> Questions { get; set; }

        [Display(Name ="Total Questions")]
        public int TotalQuestions
        {
            get
            {
                return Questions == null ? 0 : Questions.Count();
            }
        }
    }
}