using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuickQuiz.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString PreserveNewLine(this HtmlHelper htmlHelper, string value)
        {
            return new HtmlString(value == null ? value : string.Format("<pre>{0}</pre>",value.Replace(Environment.NewLine, "<br/>")));
        }
    }
}