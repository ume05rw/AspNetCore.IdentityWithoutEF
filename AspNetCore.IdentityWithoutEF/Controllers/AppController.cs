using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthNoneEf.Controllers
{
    public abstract class AppController : Controller
    {
        public AppController()
        {
            this.Out($"{this.GetType().Name}.Constructor");
        }

        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.Out($"{this.GetActionName(context)} - Start");
            base.OnActionExecuting(context);
        }

        [NonAction]
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            this.Out($"{this.GetActionName(context)} - End");
            base.OnActionExecuted(context);
        }

        protected override void Dispose(bool disposing)
        {
            this.Out($"{this.GetType().Name}.Dispose");
            base.Dispose(disposing);
        }


        /// <summary>
        /// Get "Controller.Action" string
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActionName(FilterContext context)
        {
            var routes = context.ActionDescriptor.RouteValues;
            var controller = (routes.ContainsKey("controller"))
                ? routes["controller"]
                : this.GetType().Name;
            var action = (routes.ContainsKey("action"))
                ? routes["action"]
                : context.ActionDescriptor.DisplayName.Replace("AuthNoneEf.Controllers.", "");
            return $"{controller}.{action}";
        }

        /// <summary>
        /// Output messages to console(immidiate/output window)
        /// </summary>
        /// <param name="message"></param>
        protected void Out(string message)
        {
            var typeName = this.GetType().Name
                                         .PadRight(20, ' ');
            var threadId = Thread.CurrentThread.ManagedThreadId
                                               .ToString()
                                               .PadLeft(3, ' ');

            Xb.Util.Out(typeName + " / ThID: " + threadId + "  " + message);
        }

        /// <summary>
        /// Output exception message to console
        /// </summary>
        /// <param name="ex"></param>
        protected void Out(Exception ex)
        {
            Xb.Util.Out(ex);
        }
    }
}
