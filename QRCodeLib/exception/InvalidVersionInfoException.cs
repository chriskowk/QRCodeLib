using System;
namespace ThoughtWorks.QRCode.ExceptionHandler
{
    [Serializable]
    public class InvalidVersionInfoException : VersionInformationException
    {
        internal string _message = null;
        public override string Message
        {
            get
            {
                return _message;
            }

        }

        public InvalidVersionInfoException(string message)
        {
            this._message = message;
        }
    }
}