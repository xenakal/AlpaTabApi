using AlpaTabApi.Data;
using AlpaTabApi.Dtos;
using AlpaTabApi.Models;
using AlpaTabApi.Helpers;
using AlpaTabApi.Exceptions;
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
            try
            {
                IEnumerable<AlpaTabTransaction> transactions = await repository.GetAllTransactions();
                return Results.Ok(mapper.Map<IEnumerable<TransactionReadDto>>(transactions));
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
        }

        private static async Task<IResult> GetTransactionByIdAsync(IAlpaTabDataReporitory repository, int id, IMapper mapper)
        {
            try
            {
                AlpaTabTransaction transaction = await repository.GetTransactionById(id);
                if (transaction == null)
                    return Results.NotFound(IResultMessages.TransactionNotFound(id));
                return Results.Ok(mapper.Map<TransactionReadDto>(transaction));
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
        }

        private static async Task<IResult> CreateTransactionAsync(IAlpaTabDataReporitory repository, IMapper mapper, TransactionWriteDto transaction)
        {
            try
            {
                AlpaTabTransaction newTransaction = await repository.CreateTransaction(mapper.Map<AlpaTabTransaction>(transaction));
                return Results.Created($"{Constants.TRANSACTIONS_PATH}/{newTransaction.Id}", mapper.Map<UserReadDto>(newTransaction)); 
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.SaveChangesException(e.Message));
            }
        }

        private static async Task<IResult> ModifyTransactionAsync(IAlpaTabDataReporitory repository, IMapper mapper, int id, TransactionWriteDto modifTransaction)
        {
            AlpaTabTransaction transaction;
            try
            {
                transaction = await repository.ModifyTransaction(id, mapper.Map<AlpaTabTransaction>(modifTransaction));
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
            if (transaction == null)
                return Results.NotFound(IResultMessages.TransactionNotFound(id));
            return Results.Ok(mapper.Map<UserReadDto>(transaction));

        }

        private static async Task<IResult> DeleteTransactionAsync(IAlpaTabDataReporitory repository, IMapper mapper, int id)
        {
            AlpaTabTransaction transaction;
            try
            {
                transaction = await repository.DeleteTransaction(id);
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
            if (transaction == null)
                return Results.NotFound(IResultMessages.TransactionNotFound(id));
            return Results.Ok(mapper.Map<TransactionReadDto>(transaction));
        }

        private static async Task<IResult> GetUserTransactionsAsync(IAlpaTabDataReporitory repository, IMapper mapper, string nickName)
        {
            IEnumerable<AlpaTabTransaction> transactions;
            try
            {
                transactions = await repository.GetUserTransactions(nickName);
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
            if (transactions == null)
                return Results.NotFound(IResultMessages.UserNotFound(nickName));
            return Results.Ok(mapper.Map<IEnumerable<TransactionReadDto>>(transactions));
        }
    }
}
