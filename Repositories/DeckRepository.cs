using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Interfaces;
using Npgsql;
using MonsterTradingCardsGame.Application.Configurations;

namespace MonsterTradingCardsGame.Repositories
{
    public class DeckRepository : IDeckRepository
    {
        private readonly NpgsqlConnection _connection;

        public DeckRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> Create(Deck deck)
        {
            string insertQuery =
                $"INSERT INTO \"{DatabaseConfig.SchemaName}\".\"deck\" (id, user_id) " +
                "VALUES (@Id, @UserId)";

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = insertQuery;
            AddParameters(cmd, deck);

            var wasClosed = _connection.State == System.Data.ConnectionState.Closed;
            if (wasClosed)
            {
                await _connection.OpenAsync();
            }

            var rowAffected = await cmd.ExecuteNonQueryAsync();

            if (wasClosed)
            {
                await _connection.CloseAsync();
            }

            return rowAffected > 0;
        }

        public async Task<IEnumerable<Deck>> GetAll()
        {
            var decks = new List<Deck>();
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = $"SELECT id, user_id FROM \"{DatabaseConfig.SchemaName}\".\"deck\";";
            await _connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader is not null)
            {
                while (await reader.ReadAsync())
                {
                    decks.Add(new Deck
                    {
                        Id = Guid.Parse(reader["id"].ToString()),
                        UserId = Guid.Parse(reader["user_id"].ToString())
                    });
                }
            }
            await _connection.CloseAsync();
            return decks.ToList();
        }


        public async Task<bool> Update(Deck deck)
        {
            string updateQuery =
                $"UPDATE \"{DatabaseConfig.SchemaName}\".\"deck\" SET user_id = @UserId WHERE id = @Id";

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = updateQuery;
            AddParameters(cmd, deck);
            await _connection.OpenAsync();
            var rowAffected = await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowAffected > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            string deleteQuery = $"DELETE FROM \"{DatabaseConfig.SchemaName}\".\"deck\" WHERE id = @Id";
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = deleteQuery;
            cmd.Parameters.AddWithValue("@Id", id);
            await _connection.OpenAsync();
            var rowAffected = await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowAffected > 0;
        }

        private static void AddParameters(NpgsqlCommand command, Deck deck)
        {
            var parameters = command.Parameters;

            parameters.AddWithValue("@Id", deck.Id);
            parameters.AddWithValue("@UserId", deck.UserId);
        }
    }
}