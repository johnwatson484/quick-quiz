using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickQuiz.Models.ViewModels
{
    public class QuestionAnswers
    {
        public Question Question { get; set; }

        public IList<Answer> Answers { get; set; }

        public QuestionAnswers()
        {
            Answers = new List<Answer>
            {
                new Answer(),
                new Answer(),
                new Answer(),
                new Answer()
            };
        }

        public QuestionAnswers(int quizId) : this()
        {
            Question = new Question(quizId);
        }

        public QuestionAnswers(Question question)
        {
            Question = question;
            Answers = question.Answers.ToList();
        }
    }
}