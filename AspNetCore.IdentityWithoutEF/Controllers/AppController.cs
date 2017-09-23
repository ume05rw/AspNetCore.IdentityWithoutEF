using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AuthNoneEf.Controllers
{
    public abstract class AppController : Controller
    {
        private string _thisTypeName = string.Empty;

        protected AppController()
        {
            this._thisTypeName = this.GetType().Name;
            this.Out($"{this._thisTypeName}.Constructor");
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
            this.Out($"{this._thisTypeName}.Dispose");
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
                : _thisTypeName;
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
            var typeName = _thisTypeName.PadRight(20, ' ');
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

        /// <summary>
        /// Output any value to Serialized-Json
        /// </summary>
        /// <param name="value"></param>
        protected void OutJson(object value)
        {
            Xb.Util.Out("\r\n" + JsonConvert.SerializeObject(value, Formatting.Indented));
        }
    }
}
