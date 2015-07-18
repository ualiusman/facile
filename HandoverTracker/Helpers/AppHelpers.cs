using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HandoverTracker.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString CheckBoxList(this HtmlHelper helper, IEnumerable<SelectListItem> items)
        {
            var output = new StringBuilder();

            foreach (var item in items.ToList())
            {
                output.AppendFormat("<input name='{0}' {1}  type='checkbox'>", item.Value,item.Selected?"Checked":"");
                output.AppendFormat("<label>{0}</label>", item.Text);
            }
           return new HtmlString(output.ToString());
        }

        public static string Date(this HtmlHelper helper, DateTime? date)
        {
            if(date.HasValue)
            return date.Value.ToShortDateString();
            else
            return string.Empty;
        }

        public static string Date<TModel, TProperty>( this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var modelMetadata =  ModelMetadata.FromLambdaExpression( expression, htmlHelper.ViewData);
            if (modelMetadata != null)
            {
                var date = (DateTime)modelMetadata.Model;
                return date.ToShortDateString();
            }
            else
                return string.Empty;
        }
    }
}