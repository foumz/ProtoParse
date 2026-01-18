namespace ProtoParse.Models;

public sealed class TransactionEntry
{
    public TransactionEntry(
        DateOnly bookingDate,
        string description,
        decimal amount,
        decimal? balanceAfter,
        string? counterparty,
        string? counterpartyIban,
        string? reference)
    {
        BookingDate = bookingDate;
        Description = description;
        Amount = amount;
        BalanceAfter = balanceAfter;
        Counterparty = counterparty;
        CounterpartyIban = counterpartyIban;
        Reference = reference;
    }

    public DateOnly BookingDate { get; }
    public string Description { get; }
    public decimal Amount { get; }
    public decimal? BalanceAfter { get; }
    public string? Counterparty { get; }
    public string? CounterpartyIban { get; }
    public string? Reference { get; }
}
