using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Framework.Commons.MVC
{
    /// <summary>
    ///     Class that contains helpers for manually rendering MVC views with models
    /// </summary>
    public class PartialViewUtil
    {
        /// <summary>
        ///     Populates a view with a model
        /// </summary>
        /// <param name="model">The data model to populate the view with</param>
        /// <param name="view">The view to be populated</param>
        /// <returns>An HTML string</returns>
        public static string ParseRazorViewString(object model, string view)
        {
            Guid guid = Guid.NewGuid();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + guid + ".cshtml";
            File.WriteAllText(filePath, $"@inherits System.Web.Mvc.WebViewPage<{model.GetType().FullName}>\r\n {view}");
            StringWriter st = new StringWriter();
            HttpContextWrapper context = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = new RouteData();
            ControllerContext controllerContext =
                new ControllerContext(new RequestContext(context, routeData), new FakeController());
            RazorView razor = new RazorView(controllerContext, "~/" + guid + ".cshtml", null, false, null);
            razor.Render(
                new ViewContext(controllerContext, razor, new ViewDataDictionary(model), new TempDataDictionary(), st),
                st);
            File.Delete(filePath);
            return st.ToString();
        }

        /// <summary>
        ///     Populates a view with a model
        /// </summary>
        /// <param name="model">The data model to populate the view with</param>
        /// <param name="viewName">The view to be populated</param>
        /// <param name="controller">The controller for this view</param>
        /// <returns>An HTML string</returns>
        public static string ParseRazorViewNameAsString(object model, string viewName, string controller = null)
        {
            using (StringWriter st = new StringWriter())
            {
                HttpContextWrapper context = new HttpContextWrapper(HttpContext.Current);
                RouteData routeData = new RouteData();
                routeData.Values["controller"] = controller ?? "Fake";
                ControllerContext controllerContext =
                    new ControllerContext(new RequestContext(context, routeData), new FakeController());
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controllerContext,
                    viewName);
                viewResult.View.Render(
                    new ViewContext(controllerContext, viewResult.View, new ViewDataDictionary(model),
                        new TempDataDictionary(), st), st);
                return st.ToString();
            }
        }

        /// <summary>
        ///     Populates a view with a model
        /// </summary>
        /// <param name="model">The data model to populate the view with</param>
        /// <param name="viewName">The view to be populated</param>
        /// <param name="controller">The controller for this view</param>
        /// <returns>An HTML string</returns>
        public static string RenderRazorViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult =
                    ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View,
                    controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public class FakeController : Controller
        {
        }
    }
}