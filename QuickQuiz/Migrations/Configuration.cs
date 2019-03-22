namespace QuickQuiz.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QuickQuiz.DAL.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QuickQuiz.DAL.Context context)
        {
            context.Roles.AddOrUpdate(
               p => p.Name,
               new IdentityRole { Name = "Admin" },
               new IdentityRole { Name = "User" }
             );
        }
    }
}
