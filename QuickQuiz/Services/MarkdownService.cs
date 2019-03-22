using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace QuickQuiz.Services
{
    public class MarkdownService
    {
        string root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Markdown");
        
        private string GetFileName(Guid reference)
        {
            return Path.Combine(root, string.Format("{0}.md", reference)); 
        }

        public void Upload(HttpPostedFileBase file, Guid reference)
        {
            string filename = GetFileName(reference);

            if(File.Exists(filename))
            {
                File.Delete(filename);
            }
            file.SaveAs(filename);
        }

        public void Delete(Guid reference)
        {
            string filename = GetFileName(reference);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        public string Get(Guid reference)
        {
            using (var reader = new StreamReader(GetFileName(reference)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}