using System;
using System.Collections.Generic;
public record Transaction(
    int Id,
    DateTime Date,
    decimal Amount,
    string Category
);
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Bank transfer: GHS {transaction.Amount} for {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Mobile Money payment: GHS {transaction.Amount} for {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Crypto payment: GHS {transaction.Amount} for {transaction.Category}");
    }
}
public class Account
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
        Console.WriteLine($"Transaction of GHS {transaction.Amount} applied. New balance: GHS {Balance}");
    }
}

public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction of GHS {transaction.Amount} applied. New balance: GHS {Balance}");
        }
    }
}
public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        var account = new SavingsAccount("SAV-001", 1000);

        var t1 = new Transaction(1, DateTime.Now, 100, "Groceries");
        var t2 = new Transaction(2, DateTime.Now, 250, "Utilities");
        var t3 = new Transaction(3, DateTime.Now, 300, "Entertainment");

        ITransactionProcessor processor1 = new MobileMoneyProcessor();
        ITransactionProcessor processor2 = new BankTransferProcessor();
        ITransactionProcessor processor3 = new CryptoWalletProcessor();

        processor1.Process(t1);
        account.ApplyTransaction(t1);
        _transactions.Add(t1);

        processor2.Process(t2);
        account.ApplyTransaction(t2);
        _transactions.Add(t2);

        processor3.Process(t3);
        account.ApplyTransaction(t3);
        _transactions.Add(t3);

        Console.WriteLine("\nAll Transactions:");
        foreach (var transaction in _transactions)
        {
            Console.WriteLine($"[{transaction.Id}] {transaction.Category} - GHS {transaction.Amount} on {transaction.Date}");
        }
    }
}


    class Program
{
    static void Main()
    {
        FinanceApp app = new FinanceApp();
        app.Run();
    }
}

