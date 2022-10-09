using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpaTabApi.Data;

public static class IResultMessages
{
    public static string SaveChangesException(string msg) => $"Problem commiting to DB. Message:\n {msg}";
    public static string UserNotFound(int id) => $"User not found: {id}";
    public static string UserNotFound(string nickname) => $"User not found: {nickname}";
    public static string TransactionNotFound(int id) => $"Transaction not found: {id}";
    public static string TransactionNotFound(string nickname) => $"Transaction not found: {nickname}";
    public static string DbError(string msg) => $"Problem with the database. Message: {msg}";
}
