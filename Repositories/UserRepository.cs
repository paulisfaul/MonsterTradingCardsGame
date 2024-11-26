using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Helpers;
using MonsterTradingCardsGame.Repositories.Interfaces;
using Npgsql;
using MonsterTradingCardsGame.Application.Configurations;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Helper.Database;
using MonsterTradingCardsGame.Helper.HttpServer;

namespace MonsterTradingCardsGame.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly string _tableName = $"\"{DatabaseConfig.SchemaName}\".\"user\"";
        private readonly IEnumerable<string> _columns = new List<string> { "id", "username", "password", "created_at", "last_login_at", "name", "bio", "image", "role" };

        public UserRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<(bool success, int code, string? message)> Create(User user, string hashedPassword)
        {
            await _connection.OpenAsync();
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                string insertUserQuery = QueryBuilder.BuildInsertQuery(_tableName, _columns);
                var userRowAffected = await DatabaseHelper.ExecuteNonQueryAsync(_connection, insertUserQuery, cmd => AddParameters(cmd, user, hashedPassword));
                await transaction.CommitAsync();
                if (userRowAffected > 0)
                {
                    return (true, HttpStatusCode.CREATED, null);
                }

                return (false, HttpStatusCode.BAD_REQUEST, "User could not be created.");
            }
            catch
            {
                await transaction.RollbackAsync();
                return (false, HttpStatusCode.BAD_REQUEST, "User could not be created.");
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            await _connection.OpenAsync();
            try
            {
                string query = QueryBuilder.BuildSelectQuery(_tableName, _columns);
                return await DatabaseHelper.ExecuteReaderAsync(_connection, query, cmd => { }, async reader =>
                {
                    var users = new List<User>();
                    while (await reader.ReadAsync())
                    {
                        var roleString = Convert.ToString(reader["role"]);
                        if (!Enum.TryParse<RoleEnum>(roleString, true, out var role))
                        {
                            role = RoleEnum.Player;
                        }

                        var user = new User(
                            id: Guid.Parse(reader["id"].ToString()),
                            username: Convert.ToString(reader["username"]),
                            role: role,
                            lastLoginAt: reader["last_login_at"] as DateTime?,
                            createdAt: Convert.ToDateTime(reader["created_at"]),
                            name: Convert.ToString(reader["name"]),
                            bio: Convert.ToString(reader["bio"]),
                            image: Convert.ToString(reader["image"])
                        );
                        users.Add(user);
                    }
                    return users;
                });
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<User> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            await _connection.OpenAsync();
            try
            {
                string query = QueryBuilder.BuildSelectQuery(_tableName, _columns, "id");
                return await DatabaseHelper.ExecuteReaderAsync(_connection, query, cmd => DatabaseHelper.AddParameter(cmd, "@Id", id), async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        var roleString = Convert.ToString(reader["role"]);
                        if (!Enum.TryParse<RoleEnum>(roleString, true, out var role))
                        {
                            role = RoleEnum.Player;
                        }

                        return new User(
                            id: Guid.Parse(reader["id"].ToString()),
                            username: Convert.ToString(reader["username"]),
                            role: role,
                            lastLoginAt: reader["last_login_at"] as DateTime?,
                            createdAt: Convert.ToDateTime(reader["created_at"]),
                            name: Convert.ToString(reader["name"]),
                            bio: Convert.ToString(reader["bio"]),
                            image: Convert.ToString(reader["image"])
                        );
                    }
                    return null;
                });
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<(User user,string hashedPassword)> GetByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException();
            }

            await _connection.OpenAsync();
            try
            {
                string query = QueryBuilder.BuildSelectQuery(_tableName, _columns, "username");
                return await DatabaseHelper.ExecuteReaderAsync(_connection, query, cmd => DatabaseHelper.AddParameter(cmd, "@Username", username), async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        var roleString = Convert.ToString(reader["role"]);
                        if (!Enum.TryParse<RoleEnum>(roleString, true, out var role))
                        {
                            role = RoleEnum.Player;
                        }

                        var user =  new User(
                            id: Guid.Parse(reader["id"].ToString()),
                            username: Convert.ToString(reader["username"]),
                            role: role,
                            lastLoginAt: reader["last_login_at"] as DateTime?,
                            createdAt: Convert.ToDateTime(reader["created_at"]),
                            name: Convert.ToString(reader["name"]),
                            bio: Convert.ToString(reader["bio"]),
                            image: Convert.ToString(reader["image"])
                        );

                        var hashedPassword = Convert.ToString(reader["password"]);

                        return (user, hashedPassword);

                    }
                    return (null,null);
                });
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> Update(User user)
        {
            await _connection.OpenAsync();
            try
            {
                string updateQuery = QueryBuilder.BuildUpdateQuery(_tableName, _columns.Skip(1), "id");
                var rowAffected = await DatabaseHelper.ExecuteNonQueryAsync(_connection, updateQuery, cmd => AddParameters(cmd, user, null));
                return rowAffected > 0;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> Delete(Guid userId)
        {
            await _connection.OpenAsync();
            try
            {
                string deleteQuery = QueryBuilder.BuildDeleteQuery(_tableName, "id");
                var rowAffected = await DatabaseHelper.ExecuteNonQueryAsync(_connection, deleteQuery, cmd => DatabaseHelper.AddParameter(cmd, "@Id", userId));
                return rowAffected > 0;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private static void AddParameters(NpgsqlCommand command, User user, string hashedPassword)
        {
            DatabaseHelper.AddParameter(command, "@id", user.Id);
            DatabaseHelper.AddParameter(command, "@username", user.UserCredentials.Username ?? string.Empty);
            DatabaseHelper.AddParameter(command, "@password", hashedPassword?? string.Empty);
            DatabaseHelper.AddParameter(command, "@created_at", user.UserData.CreatedAt);
            DatabaseHelper.AddParameter(command, "@last_login_at", user.UserData.LastLoginAt ?? (object)DBNull.Value);
            DatabaseHelper.AddParameter(command, "@name", user.UserData.Name ?? string.Empty);
            DatabaseHelper.AddParameter(command, "@bio", user.UserData.Bio ?? string.Empty);
            DatabaseHelper.AddParameter(command, "@image", user.UserData.Image ?? string.Empty);
            var roleParam = command.Parameters.AddWithValue("role", user.UserCredentials.Role.ToString());
            roleParam.DataTypeName = "role"; // Name des benutzerdefinierten Enum-Typs in der Datenbank
        }
    }
}