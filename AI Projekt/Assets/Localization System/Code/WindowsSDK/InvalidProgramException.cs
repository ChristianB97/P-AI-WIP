using System;

namespace Translation.WindowsSDK
{
    public class InvalidProgramException : Exception
    {
        public InvalidProgramException()
        {
        }

        public InvalidProgramException(string message)
            : base(message)
        {
        }
    }
}

