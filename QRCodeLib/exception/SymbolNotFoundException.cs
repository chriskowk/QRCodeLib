using System;
namespace ThoughtWorks.QRCode.ExceptionHandler
{
    [Serializable]
    public class SymbolNotFoundException : System.ArgumentException
    {
        internal string _message = null;

        public override string Message
        {
            get
            {
                return _message;
            }

        }

        public SymbolNotFoundException(string message)
        {
            this._message = message;
        }
    }
}