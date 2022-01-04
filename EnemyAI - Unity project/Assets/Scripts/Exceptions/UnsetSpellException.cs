using System;
using System.Runtime.Serialization;

class UnsetSpellException : Exception
{
    public UnsetSpellException() { }

    public UnsetSpellException(string message) : base(message) { }

    public UnsetSpellException(string message, Exception innerException) : base(message, innerException) { }

    protected UnsetSpellException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
