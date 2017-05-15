using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.MetadataService.Support
{
    public struct DisposableAction : IDisposable
    {
        private Action _guardFunction;

        public DisposableAction(Action guardFunction)
        {
            _guardFunction = guardFunction;
        }

        /// <summary>
        /// This function remove the guard callback, is to be used with great attenction
        /// by the caller, but is a way to avoid disposable action to be called at the 
        /// end of the scope. Remember that inside a using block there is no good way
        /// to avoid Dispose to be called.
        /// </summary>
        public void Dismiss()
        {
            _guardFunction = null;
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            _guardFunction?.Invoke();
        }

        #endregion
    }
}
