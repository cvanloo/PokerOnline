using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace PokerOnline.Data
{
    public class SqlDataAccess
    {
        private readonly string connectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">The app configuration</param>
        public SqlDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Load data from a sql query
        /// </summary>
        /// <typeparam name="T">Type of the data to load</typeparam>
        /// <typeparam name="U">Type of the parameters</typeparam>
        /// <param name="sql">Sql query</param>
        /// <param name="parameters">Parameters to pass to the query</param>
        /// <returns>Data from query, as list</returns>
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);

                return data.ToList();
            }
        }

        /// <summary>
        /// Execute a sql query
        /// </summary>
        /// <typeparam name="T">Type of the parameters</typeparam>
        /// <param name="sql">Sql query to execute</param>
        /// <param name="parameters">Parameters to pass to the query</param>
        /// <returns>Success status code.</returns>
        public async Task<int> SaveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                int success = await connection.ExecuteAsync(sql, parameters);

                return success;
            }
        }
    }
}
