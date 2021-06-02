using System;
namespace ThoughtWorks.QRCode.ExceptionHandler
{
    [Serializable]
    public class InvalidDataBlockException : System.ArgumentException
    {
        internal string _message = null;

        public override string Message
        {
            get
            {
                return _message;
            }

        }

        public InvalidDataBlockException(string message)
        {
            this._message = message;
        }
    }
}