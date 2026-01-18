using ProtoParse.Models;

namespace ProtoParse.Parsing;

public sealed class BnpParibasFortisStatementParser : IStatementParser
{
    public AccountStatement ParseFromText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ParseException("Le texte fourni est vide.");
        }

        var lines = text
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        var ibanMatch = StatementPatterns.Iban.Match(text);
        if (!ibanMatch.Success)
        {
            throw new ParseException("IBAN introuvable dans l'extrait.");
        }

        var bicMatch = StatementPatterns.Bic.Match(text);
        var dateMatch = StatementPatterns.Date.Match(text);

        if (!dateMatch.Success)
        {
            throw new ParseException("Date d'extrait introuvable.");
        }

        var accountHolder = ExtractAccountHolder(lines);
        var transactions = ExtractTransactions(text);
        var openingBalance = ExtractBalance(text, "Solde précédent");
        var closingBalance = ExtractBalance(text, "Nouveau solde");

        return new AccountStatement(
            accountHolder,
            NormalizeIban(ibanMatch.Value),
            bicMatch.Success ? bicMatch.Value : null,
            StatementPatterns.ParseDate(dateMatch.Value),
            transactions,
            openingBalance,
            closingBalance);
    }

    private static string ExtractAccountHolder(IReadOnlyList<string> lines)
    {
        var labelIndex = lines.FindIndex(line =>
            line.Contains("Titulaire", StringComparison.OrdinalIgnoreCase));

        if (labelIndex >= 0 && labelIndex + 1 < lines.Count)
        {
            return lines[labelIndex + 1];
        }

        return "Titulaire inconnu";
    }

    private static IReadOnlyList<TransactionEntry> ExtractTransactions(string text)
    {
        var matches = StatementPatterns.TransactionLine.Matches(text);
        var transactions = new List<TransactionEntry>();

        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            var bookingDate = StatementPatterns.ParseDate(match.Groups["date"].Value);
            var description = match.Groups["desc"].Value.Trim();
            var amount = StatementPatterns.ParseDecimal(match.Groups["amount"].Value);
            var balance = StatementPatterns.ParseDecimal(match.Groups["balance"].Value);

            transactions.Add(new TransactionEntry(
                bookingDate,
                description,
                amount,
                balance,
                null,
                null,
                null));
        }

        return transactions;
    }

    private static decimal? ExtractBalance(string text, string label)
    {
        var line = text
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .FirstOrDefault(l => l.Contains(label, StringComparison.OrdinalIgnoreCase));

        if (line is null)
        {
            return null;
        }

        var match = StatementPatterns.Amount.Match(line);
        if (!match.Success)
        {
            return null;
        }

        return StatementPatterns.ParseDecimal(match.Value);
    }

    private static string NormalizeIban(string value)
    {
        return value.Replace(" ", string.Empty);
    }
}
