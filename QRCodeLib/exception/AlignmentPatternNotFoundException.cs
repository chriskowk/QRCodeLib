using System;
namespace ThoughtWorks.QRCode.ExceptionHandler
{
    [Serializable]
    public class AlignmentPatternNotFoundException : System.ArgumentException
    {
        internal string _message = null;

        public override string Message
        {
            get
            {
                return _message;
            }

        }
        public AlignmentPatternNotFoundException(string message)
        {
            this._message = message;
        }
    }
}