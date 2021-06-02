using System;
namespace ThoughtWorks.QRCode.ExceptionHandler
{
    [Serializable]
    public class FinderPatternNotFoundException : System.Exception
    {
        internal string _message = null;
        public override string Message
        {
            get
            {
                return _message;
            }

        }
        public FinderPatternNotFoundException(string message)
        {
            this._message = message;
        }
    }
}