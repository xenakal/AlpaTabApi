using AlpaTabApi.Models;
using Microsoft.EntityFrameworkCore;
using AlpaTabApi.Exceptions;

namespace AlpaTabApi.Data;

public class AlpaTabDataRepository : IAlpaTabDataReporitory
{
    private readonly AlpaTabContext _context;

    public AlpaTabDataRepository(AlpaTabContext context)
    {
        _context = context;
    }

    public async Task<AlpaTabUser> CreateUser(AlpaTabUser newUser)
    {
        _context.AlpaTabUsers.Add(newUser);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(IResultMessages.SaveChangesException(e.Message), e);
        }
        return newUser;
    }

    public async Task<IEnumerable<AlpaTabUser>> GetAllUsers()
    {
        IEnumerable<AlpaTabUser> users;
        try
        {
            users = await _context.AlpaTabUsers.ToListAsync(); // can I/should I avoid enumeration here? 
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(e.Message, e);
        }
        return users;
    }

    public async Task<AlpaTabUser> GetUserByNickname(string nickname)
    {
        AlpaTabUser user;
        try
        {
            user = await _context.AlpaTabUsers.SingleOrDefaultAsync(_ => _.NickName == nickname);
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(e.Message, e);
        }
        return user is default(AlpaTabUser) ? null : user;
    }

    public async Task<AlpaTabUser> GetUserById(int id) 
    {
        AlpaTabUser user;
        try
        {
            user = await _context.AlpaTabUsers.SingleOrDefaultAsync(_ => _.Id == id);
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(e.Message, e);
        }
        return user is default(AlpaTabUser) ? null : user;
    }

    public async Task<AlpaTabUser> DeleteUser(int id)
    {
        AlpaTabUser userToRemove = await _context.AlpaTabUsers.FindAsync(id);
        if (userToRemove == null)
            return null;

        _context.AlpaTabUsers.Remove(userToRemove);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(IResultMessages.SaveChangesException(e.Message), e);
        }
        return userToRemove;
    }

    public async Task<AlpaTabUser> ModifyUser(int id, AlpaTabUser user)
    {
        AlpaTabUser existingUsr = await _context.AlpaTabUsers.FindAsync(id);
        if (existingUsr == null)
            return null;

        existingUsr.FirstName = user.FirstName ?? existingUsr.FirstName;
        existingUsr.LastName = user.LastName ?? existingUsr.LastName;
        existingUsr.UserType = user.UserType;
        existingUsr.NickName = user.NickName ?? existingUsr.NickName;
        existingUsr.Email = user.Email ?? existingUsr.Email;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(IResultMessages.SaveChangesException(e.Message), e);
        }
        return existingUsr;
    }

    public async Task<AlpaTabTransaction> CreateTransaction(AlpaTabTransaction newTransaction)
    {
        _context.TransactionsList.Add(newTransaction);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(IResultMessages.SaveChangesException(e.Message), e);
        }
        return newTransaction;
    }

    public async Task<AlpaTabTransaction> DeleteTransaction(int id)
    {
        AlpaTabTransaction transactionToRemove = await _context.TransactionsList.FindAsync(id);
        if (transactionToRemove == null) 
            return null;

        _context.TransactionsList.Remove(transactionToRemove);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(IResultMessages.SaveChangesException(e.Message), e);
        }
        return transactionToRemove;
    }

    public async Task<IEnumerable<AlpaTabTransaction>> GetAllTransactions()
    {
        try
        {
            return await _context.TransactionsList.ToListAsync(); 
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(IResultMessages.SaveChangesException(e.Message), e);
        }
    }

    public async Task<AlpaTabTransaction> GetTransactionById(int id)
    {
        AlpaTabTransaction transaction;
        try
        {
            transaction = await _context.TransactionsList.SingleOrDefaultAsync(_ => _.Id == id);
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(e.Message, e);
        }
        return transaction is default(AlpaTabTransaction) ? null : transaction;
    }

    public async Task<AlpaTabTransaction> ModifyTransaction(int id, AlpaTabTransaction transaction)
    {
        AlpaTabTransaction existingTransaction = await _context.TransactionsList.FindAsync(id);
        if (existingTransaction == null)
            return null;

        existingTransaction.Amount = transaction.Amount; 
        existingTransaction.NickName = transaction.NickName ?? existingTransaction.NickName;
        existingTransaction.TransactionType = transaction.TransactionType ?? existingTransaction.TransactionType;
        existingTransaction.Timestamp = transaction.Timestamp;
        existingTransaction.Description = transaction.Description ?? existingTransaction.Description;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(IResultMessages.SaveChangesException(e.Message), e);
        }
        return existingTransaction;
    }

    // Refactor this with specification pattern to avoid this file getting complicated 
    public async Task<IEnumerable<AlpaTabTransaction>> GetUserTransactions(string nickName)
    {
        AlpaTabUser user = await _context.AlpaTabUsers.SingleOrDefaultAsync(_ => _.NickName == nickName);
        if (user is default(AlpaTabUser))
            return null;

        IEnumerable<AlpaTabTransaction> transactions;
        try
        {
            transactions = await _context.TransactionsList.Where(_ => _.NickName == nickName).ToListAsync<AlpaTabTransaction>();
        }
        catch (Exception e)
        {
            throw new DbExceptionWrapper(e.Message, e);
        }
        return transactions;
    }

}
