using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using PokerOnline.Data;

namespace PokerOnline.Controllers
{
    public static class AuthHandler
    {
        private static readonly SqlDataAccess database = new SqlDataAccess(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        /// <summary>
        /// Store a new account in the database.
        /// </summary>
        /// <param name="user">The new account.</param>
        /// <returns>Success status code.</returns>
        public static async Task<int> CreateAccountAsync(User user)
        {
            string sql = @"INSERT INTO User (Username, PwHash) VALUES (@user.Username, user.PwHash)";

            int success = await database.SaveData(sql, user);

            return success;
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="user">The user to authenticate</param>
        /// <returns>True if the authetication was successful</returns>
        public static async Task<bool> AuthenticateAsync(User user)
        {
            string sql = @"SELECT Username, PwHash FROM User WHERE Username == @user.Username AND PwHash == @user.PwHash";

            if (null != (await database.LoadData<User, User>(sql, user)).FirstOrDefault())
            {
                return true;
            }

            return false;
        }
    }
}
