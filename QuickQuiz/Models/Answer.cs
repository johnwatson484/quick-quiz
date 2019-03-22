using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuickQuiz.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        [Display(Name = "Answer")]
        public string Text { get; set; }

        public bool Correct { get; set; }

        public virtual Question Question { get; set; }

        public Answer()
        {
        }

        public Answer(int questionId)
        {
            QuestionId = questionId;
        }
    }
}