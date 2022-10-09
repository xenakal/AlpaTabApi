using AlpaTabApi.Data;
using AlpaTabApi.Dtos;
using AlpaTabApi.Helpers;

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

        private static async Task<IResult> GetAllUsersAsync(IAlpaTabDataReporitory repository)
        {
            return await repository.GetAllUsers();
        }

        private static async Task<IResult> GetUserByIdAsync(IAlpaTabDataReporitory repository, int id)
        {
            return await repository.GetUserById(id);
        }

        private static async Task<IResult> CreateUserAsync(IAlpaTabDataReporitory repository, UserWriteDto user)
        {
            return await repository.CreateUser(user);
        }
        
        private static async Task<IResult> ModifyUserAsync(IAlpaTabDataReporitory repository, int id, UserWriteDto user)
        {
            return await repository.ModifyUser(id, user);

        }

        private static async Task<IResult> DeleteUserAsync(IAlpaTabDataReporitory repository, int id)
        {
            return await repository.DeleteUser(id);
        }
    }
}
