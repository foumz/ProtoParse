namespace ProtoParse.Parsing;

public sealed class ParseException : Exception
{
    public ParseException(string message) : base(message)
    {
    }
}
