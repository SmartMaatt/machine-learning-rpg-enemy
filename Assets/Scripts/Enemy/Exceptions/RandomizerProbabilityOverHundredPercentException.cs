using System;
using System.Runtime.Serialization;

class ProbabilityNotEqualHundredPercentException : Exception
{
    public ProbabilityNotEqualHundredPercentException(){}

    public ProbabilityNotEqualHundredPercentException(string message) : base(message){}

    public ProbabilityNotEqualHundredPercentException(string message, Exception innerException) : base(message, innerException){}

    protected ProbabilityNotEqualHundredPercentException(SerializationInfo info, StreamingContext context) : base(info, context){}
}

