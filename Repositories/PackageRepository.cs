using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Application.Configurations;
using MonsterTradingCardsGame.Helper.Database;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Helpers;
using MonsterTradingCardsGame.Repositories.Interfaces;
using Npgsql;

namespace MonsterTradingCardsGame.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly string _tableName = $"\"{DatabaseConfig.SchemaName}\".\"package\"";
        private readonly IEnumerable<string> _columns = new List<string> { "id", "user_id"};

        public PackageRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<bool> Create(Package package)
        {
            await _connection.OpenAsync();
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                string insertPackageQuery = QueryBuilder.BuildInsertQuery(_tableName, _columns);
                var packageRowAffected = await DatabaseHelper.ExecuteNonQueryAsync(_connection, insertPackageQuery, cmd => AddParameters(cmd, package));
                await transaction.CommitAsync();
                return packageRowAffected > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public Task<IEnumerable<Package>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Package> GetByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }


        //public Task<IEnumerable<Package>> GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<Package> GetByUserId(Guid userId)
        //{
        //    const string packageQuery = "SELECT id FROM packages WHERE user_id = @user_id";
        //    using var packageCmd = new NpgsqlCommand(packageQuery, _connection);
        //    packageCmd.Parameters.AddWithValue("user_id", userId);

        //    Guid packageId;
        //    using (var reader = await packageCmd.ExecuteReaderAsync())
        //    {
        //        if (!await reader.ReadAsync())
        //        {
        //            throw new InvalidOperationException("Kein Paket für diesen Benutzer gefunden.");
        //        }
        //        packageId = reader.GetGuid(0);
        //    }

        //    const string cardQuery = @"
        //        SELECT c.id, c.name, c.damage, e.name AS element_name, e.description AS element_description
        //        FROM cards c
        //        JOIN elementtypes e ON c.elementtype_id = e.id
        //        WHERE c.package_id = @package_id";
        //    using var cardCmd = new NpgsqlCommand(cardQuery, _connection);
        //    cardCmd.Parameters.AddWithValue("package_id", packageId);

        //    using var cardReader = await cardCmd.ExecuteReaderAsync();

        //    var package = new Package { Id = packageId };
        //    while (await cardReader.ReadAsync())
        //    {
        //        var cardType = cardReader.GetString(3);
        //        Card card;

        //        if (cardType == "Monster")
        //        {
        //            card = new MonsterCard();
        //            {
        //                Id = cardReader.GetGuid(0),
        //                Name = cardReader.GetString(1),
        //                Damage = cardReader.GetInt32(2),
        //                ElementType = new ElementType
        //                {
        //                    Name = cardReader.GetString(4),
        //                    Description = cardReader.GetString(5)
        //                }
        //            };
        //        }
        //        else if (cardType == "Spell")
        //        {
        //            card = new SpellCard
        //            {
        //                Id = cardReader.GetGuid(0),
        //                Name = cardReader.GetString(1),
        //                Damage = cardReader.GetInt32(2),
        //                ElementType = new ElementType
        //                {
        //                    Name = cardReader.GetString(4),
        //                    Description = cardReader.GetString(5)
        //                }
        //            };
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("Unbekannter Kartentyp: " + cardType);
        //        }

        //        package.AddCard(card);
        //    }

        //    return package;
        //}

        private static void AddParameters(NpgsqlCommand command, Package package, Guid? userId = null)
        {
            command.Parameters.AddWithValue("id", package.Id);

            if (userId.HasValue)
            {
                command.Parameters.AddWithValue("user_id", userId.Value);
            }
            else
            {
                command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Uuid).Value = DBNull.Value;
            }
        }
    }
}