using AlpaTabApi.Models;

namespace AlpaTabApi.Data;

public interface IAlpaTabDataReporitory
{
    Task<AlpaTabUser> CreateUser(AlpaTabUser user);
    Task<IEnumerable<AlpaTabUser>> GetAllUsers();
    Task<AlpaTabUser> GetUserById(int id);
    Task<AlpaTabUser> GetUserByNickname(string nickname);
    Task<AlpaTabUser> ModifyUser(int id, AlpaTabUser user);
    Task<AlpaTabUser> DeleteUser(int id);
    Task<AlpaTabTransaction> CreateTransaction(AlpaTabTransaction user);
    Task<IEnumerable<AlpaTabTransaction>> GetAllTransactions();
    Task<AlpaTabTransaction> GetTransactionById(int id);
    Task<AlpaTabTransaction> ModifyTransaction(int id, AlpaTabTransaction user);
    Task<AlpaTabTransaction> DeleteTransaction(int id);
    Task<IEnumerable<AlpaTabTransaction>> GetUserTransactions(string nickName);
}
