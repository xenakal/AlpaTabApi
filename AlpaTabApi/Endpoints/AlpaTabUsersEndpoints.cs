using AlpaTabApi.Data;
using AlpaTabApi.Dtos;
using AlpaTabApi.Models;
using AlpaTabApi.Helpers;
using AlpaTabApi.Exceptions;
using AutoMapper;

namespace AlpaTabApi.Endpoints
{
    public static class AlpaTabUsersEndpoints
    {
        public static void MapAlpaTabUsersEndpoints(this WebApplication app)
        {
            app.MapGet(Constants.USERS_PATH, GetAllUsersAsync);
                //.RequireAuthorization("CanViewAllUsers");

            app.MapGet($"{Constants.USERS_PATH}/{{id}}", GetUserByIdAsync);

            app.MapPost(Constants.USERS_PATH, CreateUserAsync);

            app.MapPut($"{Constants.USERS_PATH}/{{id}}", ModifyUserAsync);

            app.MapDelete($"{Constants.USERS_PATH}/{{id}}", DeleteUserAsync);
        }

        private static async Task<IResult> GetAllUsersAsync(IAlpaTabDataReporitory repository, IMapper mapper)
        {
            try
            {
                IEnumerable<AlpaTabUser> users = await repository.GetAllUsers();
                return Results.Ok(mapper.Map<IEnumerable<UserReadDto>>(users));
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
        }

        private static async Task<IResult> GetUserByIdAsync(IAlpaTabDataReporitory repository, IMapper mapper, int id)
        {
            try
            {
                AlpaTabUser user = await repository.GetUserById(id);
                if (user == null)
                    return Results.NotFound(IResultMessages.UserNotFound(id));
                return Results.Ok(mapper.Map<UserReadDto>(user));
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
        }

        private static async Task<IResult> GetUserByNicknameAsync(IAlpaTabDataReporitory repository, IMapper mapper, string nickname)
        {
            try
            {
                AlpaTabUser user = await repository.GetUserByNickname(nickname);
                if (user == null)
                    return Results.NotFound(IResultMessages.UserNotFound(nickname));
                return Results.Ok(mapper.Map<UserReadDto>(user));
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
        }

        private static async Task<IResult> CreateUserAsync(IAlpaTabDataReporitory repository, IMapper mapper, UserWriteDto user)
        {
            try
            {
                AlpaTabUser newUser = await repository.CreateUser(mapper.Map<AlpaTabUser>(user));
                return Results.Created($"{Constants.USERS_PATH}/{user.NickName}", mapper.Map<UserReadDto>(newUser)); 
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.SaveChangesException(e.Message));
            }
        }
        
        private static async Task<IResult> ModifyUserAsync(IAlpaTabDataReporitory repository, IMapper mapper, int id, UserWriteDto modifUser)
        {
            AlpaTabUser user;
            try
            {
                user = await repository.ModifyUser(id, mapper.Map<AlpaTabUser>(modifUser));
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
            if (user == null)
                return Results.NotFound(IResultMessages.UserNotFound(id));
            return Results.Ok(mapper.Map<UserReadDto>(user));

        }

        private static async Task<IResult> DeleteUserAsync(IAlpaTabDataReporitory repository, IMapper mapper, int id)
        {
            AlpaTabUser user;
            try
            {
                user = await repository.DeleteUser(id);
            }
            catch (DbExceptionWrapper e)
            {
                return Results.Problem(IResultMessages.DbError(e.Message));
            }
            if (user == null)
                return Results.NotFound(IResultMessages.UserNotFound(id));
            return Results.Ok(mapper.Map<UserReadDto>(user));
        }
    }
}
