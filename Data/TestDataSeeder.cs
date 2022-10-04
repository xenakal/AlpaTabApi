using AlpaTabApi.Models;

namespace AlpaTabApi.Data
{
    public class TestDataSeeder
    {
        private readonly AlpaTabContext alpaTabContext;
        private int DataLen = 5;

        public TestDataSeeder(AlpaTabContext alpaTabContext)
        {
            this.alpaTabContext = alpaTabContext;
        }

        public void SeedData()
        {
            if (!alpaTabContext.AlpaTabUsers.Any())
            {
                AlpaTabTransaction[] transactions = GenerateTransactions();
                AlpaTabUser[] users = GenerateUsers();

                alpaTabContext.AlpaTabUsers.AddRange(users);
                alpaTabContext.TransactionsList.AddRange(transactions);
                alpaTabContext.SaveChanges();
            }
        }

        private AlpaTabTransaction[] GenerateTransactions()
        {
            string[] nicknames = { "Alex", "Nicole", "Laura", "Carl", "Bob", };
            double[] amounts = Enumerable.Range(0, DataLen).Select(_ => _ *3.3-4.3).ToArray();

            var transactions = Enumerable.Range(0, DataLen).Select(_ => new AlpaTabTransaction()
            {
                NickName = nicknames[_],
                BalanceBeforeTransaction = 0,
                Amount = amounts[_],
                TransactionType = "beers",
                Timestamp = DateTime.Now,
            }).ToArray();
            return transactions;
        }

        private AlpaTabUser[] GenerateUsers()
        {
            string[] nicknames = { "Alex", "Nicole", "Laura", "Carl", "Bob", };
            string[] firstnames = { "Alex", "Nicole", "Laura", "Carl", "Bob", };
            string[] lastnames = { "Xela", "Elocin", "Arual", "Lrac", "Bob", };
            string[] emails = { "Xela@mail.com", "Elocin@mail.com", "Arual@mail.com", "Lrac@mail.com", "Bob@mail.com", };
            int[] balances = Enumerable.Range(0, DataLen).Select(_ => _ *100-43).ToArray();

            var users = Enumerable.Range(0, DataLen).Select(_ => new AlpaTabUser()
            {
                NickName = nicknames[_],
                FirstName = firstnames[_],
                LastName = lastnames[_],
                Email = emails[_],
                UserType = 0,
                Balance = 0,
            }).ToArray();
            return users;
        }

    }
}
