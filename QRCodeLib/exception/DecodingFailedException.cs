using System;
namespace ThoughtWorks.QRCode.ExceptionHandler
{

    // Possible Exceptions
    //
    //DecodingFailedException
    //- SymbolNotFoundException
    //  - FinderPatternNotFoundException
    //  - AlignmentPatternNotFoundException
    //- SymbolDataErrorException
    //  - IllegalDataBlockException
    //	- InvalidVersionInfoException
    //- UnsupportedVersionException

    [Serializable]
    public class DecodingFailedException : System.ArgumentException
    {
        internal string _message = null;

        public override string Message
        {
            get
            {
                return _message;
            }

        }

        public DecodingFailedException(string message)
        {
            this._message = message;
        }
    }
}