using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Framework.Commons.Utilities;

namespace Framework.UI.Web.MvcHtmlHelpers
{
    public static class ExtensionMethods
    {
        /// <summary>
        ///     Validation message extension to allow custom prefix
        /// </summary>
        /// <typeparam name="TModel">The model type</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="htmlHelper">The HTML helper class</param>
        /// <param name="prefix">The prefix to prepend to the name and id</param>
        /// <param name="expression">The LINQ expression to get the value</param>
        /// <param name="validationMessage">The custom validation message to display</param>
        /// <param name="htmlAttributes">The attributes to be added to the HTML</param>
        /// <returns>An HTML string</returns>
        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            string prefix, Expression<Func<TModel, TProperty>> expression, string validationMessage,
            object htmlAttributes)
        {
            return htmlHelper.ValidationMessage($"{prefix}{ExpressionHelper.GetExpressionText(expression)}",
                validationMessage,
                htmlAttributes);
        }

        /// <summary>
        ///     Hidden input extension to allow custom prefix
        /// </summary>
        /// <typeparam name="TModel">The model type</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="htmlHelper">The HTML helper class</param>
        /// <param name="prefix">The prefix to prepend to the name and id</param>
        /// <param name="expression">The LINQ expression to get the value</param>
        /// <param name="htmlAttributes">The attributes to be added to the HTML</param>
        /// <returns>An HTML string</returns>
        public static MvcHtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            return htmlHelper.Hidden($"{prefix}{ExpressionHelper.GetExpressionText(expression)}",
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model,
                htmlAttributes);
        }

        /// <summary>
        ///     Text area extension to allow custom prefix
        /// </summary>
        /// <typeparam name="TModel">The model type</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="htmlHelper">The HTML helper class</param>
        /// <param name="prefix">The prefix to prepend to the name and id</param>
        /// <param name="expression">The LINQ expression to get the value</param>
        /// <param name="htmlAttributes">The attributes to be added to the HTML</param>
        /// <returns>An HTML string</returns>
        public static MvcHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string value = modelMetadata.Model?.ToString() ?? string.Empty;

            return htmlHelper.TextArea($"{prefix}{ExpressionHelper.GetExpressionText(expression)}", value,
                htmlAttributes);
        }

        /// <summary>
        ///     Text box extension to allow custom prefix
        /// </summary>
        /// <typeparam name="TModel">The model type</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="htmlHelper">The HTML helper class</param>
        /// <param name="prefix">The prefix to prepend to the name and id</param>
        /// <param name="expression">The LINQ expression to get the value</param>
        /// <param name="htmlAttributes">The attributes to be added to the HTML</param>
        /// <returns>An HTML string</returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return htmlHelper.TextBox($"{prefix}{ExpressionHelper.GetExpressionText(expression)}",
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model,
                htmlAttributes);
        }

        /// <summary>
        ///     Drop Down extension to allow custom prefix
        /// </summary>
        /// <typeparam name="TModel">The model type</typeparam>
        /// <typeparam name="TValue">The type of the property</typeparam>
        /// <param name="helper">The HTML helper class</param>
        /// <param name="prefix">The prefix to prepend to the name and id</param>
        /// <param name="expression">The LINQ expression to get the value</param>
        /// <param name="selectList">The select list containing to possible values for the dropdown</param>
        /// <param name="htmlAttributes">The attributes to be added to the HTML</param>
        /// <returns>An HTML string</returns>
        public static MvcHtmlString DropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper, string prefix,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList,
            object htmlAttributes = null)
        {
            var method = expression.Compile();
            string value = method(helper.ViewData.Model) as string;

            IEnumerable<SelectListItem> selectListItems = selectList.ToList();
            foreach (SelectListItem item in selectListItems)
                if (item.Value.EqualsIgnoreCase(value))
                    item.Selected = true;

            return helper.DropDownList($"{prefix}{ExpressionHelper.GetExpressionText(expression)}", selectListItems,
                htmlAttributes);
        }

        /// <summary>
        ///     Check box extension to allow custom prefix
        /// </summary>
        /// <typeparam name="TModel">The model type</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="htmlHelper">The HTML helper class</param>
        /// <param name="prefix">The prefix to prepend to the name and id</param>
        /// <param name="expression">The LINQ expression to get the value</param>
        /// <param name="htmlAttributes">The attributes to be added to the HTML</param>
        /// <returns>An HTML string</returns>
        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            return htmlHelper.CheckBox($"{prefix}{ExpressionHelper.GetExpressionText(expression)}",
                (bool) ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model,
                htmlAttributes);
        }
    }
}