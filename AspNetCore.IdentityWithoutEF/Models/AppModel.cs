using System;
using System.Threading;
using Newtonsoft.Json;

namespace AuthNoneEf.Models
{
    public abstract class AppModel
    {
        private string _thisTypeName = string.Empty;

        protected AppModel()
        {
            this._thisTypeName = this.GetType().Name;
            this.Out($"{this._thisTypeName}.Constructor");
        }

        /// <summary>
        /// 破棄する
        /// </summary>
        public void Dispose()
        {
            this.Out($"{this._thisTypeName}.Dispose");
            this._disposed = true;
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private bool _disposed;



        /// <summary>
        /// Output messages to console(immidiate/output window)
        /// </summary>
        /// <param name="message"></param>
        protected void Out(string message)
        {
            var typeName = this._thisTypeName.PadRight(20, ' ');
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
