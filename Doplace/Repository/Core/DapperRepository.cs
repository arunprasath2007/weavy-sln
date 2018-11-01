using Dapper;
using Doplace.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Doplace.Repository.Core
{
    class DapperRepository : IRepository
    {
        private const int COMMAND_TIME_OUT = 300;

        public void Execute(string sql, object param)
        {
            using (var connection = new SqlConnection(ConfigurationConstants.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();
                    connection.Execute(sql, param, commandTimeout: COMMAND_TIME_OUT);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<TReturn> Query<TReturn>(string sql, object param)
        {
            using (var connection = new SqlConnection(ConfigurationConstants.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();
                    return connection.Query<TReturn>(sql, param, commandTimeout: COMMAND_TIME_OUT);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, object param, Func<TFirst, TSecond, TReturn> func)
        {
            using (var connection = new SqlConnection(ConfigurationConstants.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();
                    return connection.Query(sql, func, param, commandTimeout: COMMAND_TIME_OUT);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, object param, Func<TFirst, TSecond, TThird, TReturn> func)
        {
            using (var connection = new SqlConnection(ConfigurationConstants.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();
                    return connection.Query(sql, func, param, commandTimeout: COMMAND_TIME_OUT);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, object param, Func<TFirst, TSecond, TThird, TFourth, TReturn> func)
        {
            using (var connection = new SqlConnection(ConfigurationConstants.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();
                    return connection.Query(sql, func, param, commandTimeout: COMMAND_TIME_OUT);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<TReturn> Query<TReturn>(string sql, object param, Type[] types, Func<object[], TReturn> func)
        {
            using (var connection = new SqlConnection(ConfigurationConstants.CONNECTION_STRING))
            {
                try
                {
                    connection.Open();
                    return connection.Query(sql, types, func, param, commandTimeout: COMMAND_TIME_OUT);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
