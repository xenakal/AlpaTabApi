using AlpaTabApi.Models;

namespace AlpaTabApi.Data
{
    public interface IAlpaTabDataReporitory
    {
        Task<AlpaTabUser> CreateUser(AlpaTabUser user);
        Task<List<AlpaTabUser>> GetAllUsers();
        Task<AlpaTabUser> GetUserById(int id);
        Task<AlpaTabUser> ModifyUser(int id, AlpaTabUser user);
        Task<AlpaTabUser> DeleteUser(int id);
        Task<AlpaTabTransaction> CreateTransaction(AlpaTabTransaction user);
        Task<List<AlpaTabTransaction>> GetAllTransactions();
        Task<AlpaTabTransaction> GetTransactionById(int id);
        Task<AlpaTabTransaction> ModifyTransaction(int id, AlpaTabTransaction user);
        Task<AlpaTabTransaction> DeleteTransaction(int id);
        Task<List<AlpaTabTransaction>> GetUserTransactions(string nickName);
    }
}
