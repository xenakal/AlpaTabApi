using AlpaTabApi.Models;
using AlpaTabApi.Dtos;

namespace AlpaTabApi.Data;

public interface IAlpaTabDataReporitory
{
    Task<IResult> CreateUser(UserWriteDto user);
    Task<IResult> GetAllUsers();
    Task<IResult> GetUserById(int id);
    Task<IResult> GetUserByNickname(string nickname);
    Task<IResult> ModifyUser(int id, UserWriteDto user);
    Task<IResult> DeleteUser(int id);
    Task<IResult> CreateTransaction(TransactionWriteDto user);
    Task<IEnumerable<AlpaTabTransaction>> GetAllTransactions();
    Task<IResult> GetTransactionById(int id);
    Task<IResult> ModifyTransaction(int id, TransactionWriteDto user);
    Task<IResult> DeleteTransaction(int id);
    Task<IResult> GetUserTransactions(string nickName);
}
