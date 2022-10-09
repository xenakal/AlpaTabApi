using AlpaTabApi.Data;
using AlpaTabApi.Dtos;
using AlpaTabApi.Models;
using AlpaTabApi.Helpers;
using AutoMapper;

namespace AlpaTabApi.Endpoints
{
    public static class AlpaTabTransactionsEndpoints
    {
        public static void MapAlpaTabTransactionsEndpoints(this WebApplication app)
        {
            app.MapGet($"{Constants.TRANSACTIONS_PATH}", GetAllTransactionsAsync);
            //.RequireAuthorization("CanViewAllUsers");

            app.MapGet($"{Constants.TRANSACTIONS_PATH}/{{id}}", GetTransactionByIdAsync);

            app.MapPost($"{Constants.TRANSACTIONS_PATH}", CreateTransactionAsync);

            app.MapPut($"{Constants.TRANSACTIONS_PATH}/{{id}}", ModifyTransactionAsync);

            app.MapDelete($"{Constants.TRANSACTIONS_PATH}/{{id}}", DeleteTransactionAsync);

            app.MapGet($"{Constants.TRANSACTIONS_PATH}{Constants.USERS_PATH}/{{nickname}}", GetUserTransactionsAsync);
        }

        private static async Task<IResult> GetAllTransactionsAsync(IAlpaTabDataReporitory repository, IMapper mapper)
        {
            IEnumerable<AlpaTabTransaction> transactions = await repository.GetAllTransactions();
            return Results.Ok(mapper.Map<IEnumerable<TransactionReadDto>>(transactions));
        }

        private static async Task<IResult> GetTransactionByIdAsync(IAlpaTabDataReporitory repository, int id)
        {
            return await repository.GetTransactionById(id);
        }

        private static async Task<IResult> CreateTransactionAsync(IAlpaTabDataReporitory repository, TransactionWriteDto transaction)
        {
            return await repository.CreateTransaction(transaction);
        }

        private static async Task<IResult> ModifyTransactionAsync(IAlpaTabDataReporitory repository, int id, TransactionWriteDto transaction)
        {
            return await repository.ModifyTransaction(id, transaction);

        }

        private static async Task<IResult> DeleteTransactionAsync(IAlpaTabDataReporitory repository, int id)
        {
            return await repository.DeleteTransaction(id);
        }

        private static async Task<IResult> GetUserTransactionsAsync(IAlpaTabDataReporitory repository, string nickName)
        {
            return await repository.GetUserTransactions(nickName);
        }
    }
}
