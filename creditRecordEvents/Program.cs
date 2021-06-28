using System;

namespace creditRecordEvents
{
    class Program
    {
        static void Main()
        {
            var creditRecordRepository = new CreditsRecordRepository();

            var key = string.Empty;
            while (key != "X")
            {
                Console.WriteLine("A: Add Credit"); 
                Console.WriteLine("S: Spend Credit");
                Console.WriteLine("X: Expire Credit");
                Console.WriteLine("R: Remaining Credit");
                Console.WriteLine("E: Events");
                Console.WriteLine("> ");
                key = Console.ReadLine()?.ToUpperInvariant();
                Console.WriteLine();

                var creditId = GetCreditIdFromConsole();
                var creditRecord = creditRecordRepository.Get(creditId);

                switch (key)
                {
                    case "A":
                        var addInput = GetAmount();
                        if (addInput.IsValid)
                        {
                            creditRecord.AddCredit(addInput.Amount);
                            Console.WriteLine($"{creditId} Added: £{addInput.Amount}");
                        }
                        break;
                    case "S":
                        var spendInput = GetAmount();
                        if (spendInput.IsValid)
                        {
                            creditRecord.SpendCredit(spendInput.Amount);
                            Console.WriteLine($"{creditId} Spent: £{spendInput.Amount}");
                        }
                        break;
                    case "X":
                        var expireInput = GetAmount();
                        if (expireInput.IsValid)
                        {
                            var reason = GetExpiryReason();
                            creditRecord.ExpireCredit(expireInput.Amount, reason);
                            Console.WriteLine($"{creditId} Expired: £{expireInput.Amount} Reason: {reason}");
                        }
                        break;
                    case "R":
                        var currentBalance = creditRecord.GetRemainingCredit();
                        Console.WriteLine($"{creditId} Credit Balance: {currentBalance}");
                        break;
                    case "E":
                        Console.WriteLine($"Events: {creditId}");
                        foreach (var evnt in creditRecord.GetEvents())
                        {
                            switch (evnt)
                            {
                                case CreditAdded addCredit:
                                    Console.WriteLine($"{addCredit.DateTime:g} {creditId} Added: £{addCredit.CreditAmount} ");
                                    break;
                                case CreditSpent spendCredit:
                                    Console.WriteLine($"{spendCredit.DateTime:g} {creditId} Spent: £{spendCredit.CreditAmount} ");
                                    break;
                                case CreditExpired expireCredit:
                                    Console.WriteLine($"{expireCredit.DateTime:g} {creditId} Added: £{expireCredit.CreditAmount} Reason: {expireCredit.Reason}");
                                    break;
                            }
                        }
                        break;
                }
                creditRecordRepository.Save(creditRecord);
                Console.ReadLine();
                Console.WriteLine();
            }
        }

        private static string GetExpiryReason()
        {
            Console.WriteLine("Reason: ");
            return Console.ReadLine();
        }

        private static (int Amount , bool IsValid) GetAmount()
        {
            Console.WriteLine("Credit Amount: ");
            if (int.TryParse(Console.ReadLine(), out var amount))
            {
                return (amount, true);
            }
            else
            {
                Console.WriteLine("Invalid Credit Amount.");
                return (0, false);
            }
        }

        private static string GetCreditIdFromConsole()
        {
            Console.Write("ID: ");
            return Console.ReadLine();
        }
    }
}