using System.Globalization;
using System.Text.RegularExpressions;

namespace ProtoParse.Parsing;

internal static class StatementPatterns
{
    internal static readonly Regex Iban = new(
        @"\bBE\d{2}\s?\d{4}\s?\d{4}\s?\d{4}\b",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    internal static readonly Regex Bic = new(
        @"\b[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?\b",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    internal static readonly Regex Date = new(
        @"\b(?<day>\d{2})/(?<month>\d{2})/(?<year>\d{4})\b",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    internal static readonly Regex Amount = new(
        @"(?<sign>-)?(?<value>\d{1,3}(?:\.\d{3})*,\d{2})",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    internal static readonly Regex TransactionLine = new(
        @"^(?<date>\d{2}/\d{2}/\d{4})\s+(?<desc>.+?)\s+(?<amount>-?\d{1,3}(?:\.\d{3})*,\d{2})\s+(?<balance>-?\d{1,3}(?:\.\d{3})*,\d{2})$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

    internal static decimal ParseDecimal(string value)
    {
        return decimal.Parse(value.Replace(".", string.Empty), new CultureInfo("fr-BE"));
    }

    internal static DateOnly ParseDate(string value)
    {
        var culture = new CultureInfo("fr-BE");
        return DateOnly.ParseExact(value, "dd/MM/yyyy", culture);
    }
}
