using System;
using System.Threading;

namespace AuthNoneEf.Models
{
    public class AppModel
    {
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
