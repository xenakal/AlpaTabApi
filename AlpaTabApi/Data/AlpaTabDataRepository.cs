using AlpaTabApi.Models;
using AlpaTabApi.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AlpaTabApi.Data;

public class AlpaTabDataRepository : IAlpaTabDataReporitory
{
    private readonly AlpaTabContext _context;
    private readonly IMapper _mapper;

    public AlpaTabDataRepository(AlpaTabContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IResult> CreateUser(UserWriteDto user)
    {
        AlpaTabUser newUser = _mapper.Map<AlpaTabUser>(user);
        _context.AlpaTabUsers.Add(newUser);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Results.Problem(IResultMessages.SaveChangesException(e.Message));
        }
        return Results.Created($"users/{user.NickName}", _mapper.Map<UserReadDto>(newUser)); 
    }

    public async Task<IResult> GetAllUsers()
    {
        IEnumerable<AlpaTabUser> users = await _context.AlpaTabUsers.ToListAsync(); // can I/should I avoid enumeration here? 
        return Results.Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
    }

    public async Task<IResult> GetUserByNickname(string nickname)
    {
        AlpaTabUser user = await _context.AlpaTabUsers.SingleOrDefaultAsync(_ => _.NickName == nickname);
        if (user == default)
            return Results.NotFound(IResultMessages.UserNotFound(nickname));
        return Results.Ok(_mapper.Map<UserReadDto>(user));
    }

    public async Task<IResult> GetUserById(int id) 
    {
        //if (await _context.AlpaTabUsers.FindAsync(id) is AlpaTabUser user) BENCHMARK TO COMPARE
        var user = await _context.AlpaTabUsers.FindAsync(id);
        if (user == null)
            return Results.NotFound(IResultMessages.UserNotFound(id));
        return Results.Ok(_mapper.Map<UserReadDto>(user));
    }

    public async Task<IResult> DeleteUser(int id)
    {
        AlpaTabUser userToRemove = await _context.AlpaTabUsers.FindAsync(id);
        if (userToRemove == null) 
            return Results.NotFound(IResultMessages.UserNotFound(id));

        _context.AlpaTabUsers.Remove(userToRemove);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Results.Problem(IResultMessages.SaveChangesException(e.Message));
        }
        return Results.Ok(_mapper.Map<UserReadDto>(userToRemove));
    }

    public async Task<IResult> ModifyUser(int id, UserWriteDto user)
    {
        AlpaTabUser existingUsr = await _context.AlpaTabUsers.FindAsync(id);
        if (existingUsr == null) 
            return Results.NotFound(IResultMessages.UserNotFound(id));

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
            return Results.Problem(IResultMessages.SaveChangesException(e.Message));
        }

        return Results.Ok(_mapper.Map<UserReadDto>(existingUsr));
    }

    public async Task<IResult> CreateTransaction(TransactionWriteDto transaction)
    {
        AlpaTabTransaction newTransaction = _mapper.Map<AlpaTabTransaction>(transaction);
        _context.TransactionsList.Add(newTransaction);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Results.Problem(IResultMessages.SaveChangesException(e.Message));
        }
        return Results.Ok(_mapper.Map<TransactionReadDto>(newTransaction)); 
    }

    public async Task<IResult> DeleteTransaction(int id)
    {
        AlpaTabTransaction transactionToRemove = await _context.TransactionsList.FindAsync(id);
        if (transactionToRemove == null) 
            return Results.NotFound(IResultMessages.UserNotFound(id));

        _context.TransactionsList.Remove(transactionToRemove);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Results.Problem(IResultMessages.SaveChangesException(e.Message));
        }
        return Results.Ok(_mapper.Map<TransactionReadDto>(transactionToRemove));
    }

    public async Task<IEnumerable<AlpaTabTransaction>> GetAllTransactions()
    {
        return await _context.TransactionsList.ToListAsync(); 
    }

    public async Task<IResult> GetTransactionById(int id)
    {
        AlpaTabTransaction transaction = await _context.TransactionsList.FindAsync(id);
        if (transaction == null)
            return Results.NotFound(IResultMessages.TransactionNotFound(id));
        return Results.Ok(_mapper.Map<TransactionReadDto>(transaction));
    }

    public async Task<IResult> ModifyTransaction(int id, TransactionWriteDto transaction)
    {
        AlpaTabTransaction existingTransaction = await _context.TransactionsList.FindAsync(id);
        if (existingTransaction == null)
            return Results.NotFound(IResultMessages.TransactionNotFound(id));

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
            return Results.Problem(IResultMessages.SaveChangesException(e.Message));
        }
        return Results.Ok(_mapper.Map<TransactionReadDto>(transaction));
    }

    // Refactor this with specification pattern to avoid this file getting complicated 
    public async Task<IResult> GetUserTransactions(string nickName)
    {
        var transactions = await _context.TransactionsList.Where(_ => _.NickName == nickName).ToListAsync<AlpaTabTransaction>();
        return Results.Ok(_mapper.Map<TransactionReadDto>(transactions));
    }

}
