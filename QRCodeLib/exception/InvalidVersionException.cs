using System;
namespace ThoughtWorks.QRCode.ExceptionHandler
{
    [Serializable]
    public class InvalidVersionException : VersionInformationException
    {
        internal string _message;
        public override string Message
        {
            get
            {
                return _message;
            }

        }

        public InvalidVersionException(string message)
        {
            this._message = message;
        }
    }
}