using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuickQuiz.Models.Session
{
    public class Assessment
    {
        public string Title { get; set; }

        public IList<AssessmentQuestion> AssessmentQuestions { get; set; }

        public int Questions
        {
            get
            {
                return AssessmentQuestions.Count();
            }
        }

        public int Answered
        {
            get
            {
                return AssessmentQuestions.Where(x => x.Answered).Count();
            }
        }

        public int Correct
        {
            get
            {
                return AssessmentQuestions.Where(x => x.Correct).Count();
            }
        }

        public int Incorrect
        {
            get
            {
                return AssessmentQuestions.Where(x => x.Answered && !x.Correct).Count();
            }
        }
                
        public decimal Performance
        {
            get
            {
                return Math.Round((decimal.Divide(Correct, Answered)) * 100, 2);
            }
        }

        public Assessment()
        {
        }

        public Assessment(List<Question> questions)
        {
            Title = questions.FirstOrDefault()?.Quiz?.Title;
            AssessmentQuestions = new List<AssessmentQuestion>();

            foreach(var question in questions)
            {
                AssessmentQuestions.Add(new AssessmentQuestion(question));
            }
        }

        public AssessmentQuestion GetNext()
        {
            return AssessmentQuestions.Where(x => !x.Answered).OrderBy(x => x.Order).FirstOrDefault();
        }

        public bool IsComplete
        {
            get
            {
                return AssessmentQuestions.Where(x => !x.Answered).Count() == 0;
            }
        }
    }

    public class AssessmentQuestion
    {
        public Question Question { get; set; }

        public bool Answered { get; set; }

        public bool Correct { get; set; }

        public Guid Order { get; set; }

        public AssessmentQuestion()
        {
        }

        public AssessmentQuestion(Question question)
        {
            Question = question;
            Order = Guid.NewGuid();
        }
    }
}