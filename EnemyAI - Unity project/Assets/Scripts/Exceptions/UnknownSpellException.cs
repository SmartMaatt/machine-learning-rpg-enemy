using System;
using System.Runtime.Serialization;

class UnknownSpellException : Exception
{
    public UnknownSpellException() { }

    public UnknownSpellException(string message) : base(message) { }

    public UnknownSpellException(string message, Exception innerException) : base(message, innerException) { }

    protected UnknownSpellException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
