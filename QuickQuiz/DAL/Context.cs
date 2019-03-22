using Microsoft.AspNet.Identity.EntityFramework;
using QuickQuiz.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QuickQuiz.DAL
{
    public class Context:IdentityDbContext<ApplicationUser>
    {
        public Context()
            : base("Context", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Quiz> Quizzes { get; set; }

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<Answer> Answers { get; set; }

        public static Context Create()
        {
            return new Context();
        }
    }
}