using AlpaTabApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlpaTabApi.Data
{
    public class AlpaTabDataRepository : IAlpaTabDataReporitory
    {
        private readonly AlpaTabContext _context;

        public AlpaTabDataRepository(AlpaTabContext context)
        {
            _context = context;
        }

        public async Task<AlpaTabUser> CreateUser(AlpaTabUser user)
        {
            _context.AlpaTabUsers.Add(user);
            await _context.SaveChangesAsync();
            return (AlpaTabUser)Results.Ok(user); 
            // TODO: is this the correct way to use Task<..>?
        }

        public async Task<List<AlpaTabUser>> GetAllUsers()
        {
            return await _context.AlpaTabUsers.ToListAsync();
        }

        // TODO: return IResult<> or wtv?
        public async Task<AlpaTabUser> GetUserById(int id) 
        {
            if (await _context.AlpaTabUsers.FindAsync(id) is AlpaTabUser user)
                return (AlpaTabUser)Results.Ok(user);
            return (AlpaTabUser)Results.NotFound($"User not found: {id}");
            //return (AlpaTabUser)(await _context.AlpaTabUsers.FindAsync(id) is AlpaTabUser user ? 
            //                      Results.Ok(user) : Results.NotFound($"User not found: {id}"));
        }
        public async Task<AlpaTabUser> DeleteUser(int id)
        {
            AlpaTabUser userToRemove = await _context.AlpaTabUsers.FindAsync(id);
            if (userToRemove == null) return (AlpaTabUser)Results.NotFound($"User not found: {id}");

            _context.AlpaTabUsers.Remove(userToRemove);
            await _context.SaveChangesAsync();
            return (AlpaTabUser)Results.Ok(userToRemove);
        }

        public async Task<AlpaTabUser> ModifyUser(int id, AlpaTabUser user)
        {
            AlpaTabUser existingUsr = await _context.AlpaTabUsers.FindAsync(id);
            if (existingUsr == null) return (AlpaTabUser)Results.NotFound($"User not found: {id}");

            existingUsr.FirstName = user.FirstName;
            existingUsr.LastName = user.LastName;
            existingUsr.UserType = user.UserType;
            existingUsr.NickName = user.NickName;
            existingUsr.Email = user.Email;
            await _context.SaveChangesAsync();

            return (AlpaTabUser)Results.Ok(user);
        }

        public async Task<AlpaTabTransaction> CreateTransaction(AlpaTabTransaction transaction)
        {
            _context.TransactionsList.Add(transaction);
            await _context.SaveChangesAsync();
            return (AlpaTabTransaction)Results.Ok(transaction); 
            // TODO: is this the correct way to use Task<..>?
        }

        public async Task<AlpaTabTransaction> DeleteTransaction(int id)
        {
            AlpaTabTransaction transactionToRemove = await _context.TransactionsList.FindAsync(id);
            if (transactionToRemove == null) return (AlpaTabTransaction)Results.NotFound($"User not found: {id}");

            _context.TransactionsList.Remove(transactionToRemove);
            await _context.SaveChangesAsync();
            return (AlpaTabTransaction)Results.Ok(transactionToRemove);
        }

        public async Task<List<AlpaTabTransaction>> GetAllTransactions()
        {
            return await _context.TransactionsList.ToListAsync();
        }

        public async Task<AlpaTabTransaction> GetTransactionById(int id) => await _context.TransactionsList.FindAsync(id);

        // TODO: use ErrorOr component ? 
        public async Task<AlpaTabTransaction> ModifyTransaction(int id, AlpaTabTransaction transaction)
        {
            AlpaTabTransaction existingTransaction = await _context.TransactionsList.FindAsync(id);
            if (existingTransaction == null) return existingTransaction; // TODO: HANDLE ERRORS

            existingTransaction.Amount = transaction.Amount;
            existingTransaction.NickName = transaction.NickName;
            existingTransaction.TransactionType = transaction.TransactionType;
            existingTransaction.Timestamp = transaction.Timestamp;
            existingTransaction.BalanceBeforeTransaction = 0; // TODO
            existingTransaction.Description = transaction.Description;
            await _context.SaveChangesAsync();

            return transaction;
        }

        // Refactor this with specification pattern to avoid this file getting complicated
        public async Task<List<AlpaTabTransaction>> GetUserTransactions(string nickName)
        {
            IList<AlpaTabTransaction> transactions = await _context.TransactionsList.Where(_ => _.NickName == nickName).ToListAsync();
            return (List<AlpaTabTransaction>)Results.Ok(transactions);
        }

    }
}
