using System.Collections.ObjectModel;

namespace ProtoParse.Models;

public sealed class AccountStatement
{
    public AccountStatement(
        string accountHolder,
        string iban,
        string? bic,
        DateOnly statementDate,
        IReadOnlyList<TransactionEntry> transactions,
        decimal? openingBalance,
        decimal? closingBalance)
    {
        AccountHolder = accountHolder;
        Iban = iban;
        Bic = bic;
        StatementDate = statementDate;
        Transactions = new ReadOnlyCollection<TransactionEntry>(transactions.ToList());
        OpeningBalance = openingBalance;
        ClosingBalance = closingBalance;
    }

    public string AccountHolder { get; }
    public string Iban { get; }
    public string? Bic { get; }
    public DateOnly StatementDate { get; }
    public IReadOnlyList<TransactionEntry> Transactions { get; }
    public decimal? OpeningBalance { get; }
    public decimal? ClosingBalance { get; }
}
