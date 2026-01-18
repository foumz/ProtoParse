using ProtoParse.Models;

namespace ProtoParse.Parsing;

public interface IStatementParser
{
    AccountStatement ParseFromText(string text);
}
