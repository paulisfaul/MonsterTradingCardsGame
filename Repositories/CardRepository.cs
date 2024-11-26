using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Application.Configurations;
using MonsterTradingCardsGame.Helper.Database;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Helpers;
using MonsterTradingCardsGame.Repositories.Interfaces;
using Npgsql;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace MonsterTradingCardsGame.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly string _tableNameCard = $"\"{DatabaseConfig.SchemaName}\".\"card\"";
        private readonly string _tableNameMonsterCard = $"\"{DatabaseConfig.SchemaName}\".\"monstercard\"";
        private readonly string _tableNameSpellCard = $"\"{DatabaseConfig.SchemaName}\".\"spellcard\"";

        private readonly IEnumerable<string> _columnsCard = new List<string> { "id", "name", "damage", "package_id", "elementtype" };
        private readonly IEnumerable<string> _columnsMonsterCard = new List<string> { "id"};
        private readonly IEnumerable<string> _columnsSpellCard = new List<string> { "id" };

        public CardRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> Create(Card card, Guid? packageId = null)
        {
            try
            {
                string insertCardQuery = QueryBuilder.BuildInsertQuery(_tableNameCard, _columnsCard);
                var cardRowAffected = await DatabaseHelper.ExecuteNonQueryAsync(_connection, insertCardQuery, cmd => AddCardParameters(cmd, card, packageId));

                if (cardRowAffected == 0)
                {
                    return false;
                }

                if (card is MonsterCard)
                {
                    string insertMonsterCardQuery = QueryBuilder.BuildInsertQuery(_tableNameMonsterCard, _columnsMonsterCard);
                    var monsterCardRowAffected = await DatabaseHelper.ExecuteNonQueryAsync(_connection, insertMonsterCardQuery, cmd => AddMonsterCardParameters(cmd, card));

                    if (monsterCardRowAffected == 0)
                    {
                        return false;
                    }
                }

                if (card is SpellCard)
                {
                    string insertSpellCardQuery = QueryBuilder.BuildInsertQuery(_tableNameSpellCard, _columnsSpellCard);
                    var spellCardRowAffected = await DatabaseHelper.ExecuteNonQueryAsync(_connection, insertSpellCardQuery, cmd => AddSpellCardParameters(cmd, card));

                    if (spellCardRowAffected == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateCardsForPackage(Package package)
        {
            await _connection.OpenAsync();
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                foreach (var card in package)
                {
                    var result = await Create(card, package.Id);
                    if (!result)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                await transaction.CommitAsync();
                return true;
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

        public async Task<IEnumerable<Card>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Package> GetAllPackage(Guid packageId)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> Update(Card card)
        {
            throw new NotImplementedException();
        }



        public async Task<bool> Delete(Guid cardId)
        {
            throw new NotImplementedException();
        }

        private static void AddCardParameters(NpgsqlCommand command, Card card, Guid? packageId)
        {
            command.Parameters.AddWithValue("id", card.Id);
            command.Parameters.AddWithValue("name", card.Name);
            command.Parameters.AddWithValue("damage", card.Damage);
            if (packageId != null)
            {
                command.Parameters.AddWithValue("package_id", packageId);
            }
            else
            {
                command.Parameters.Add("package_id", NpgsqlTypes.NpgsqlDbType.Uuid).Value = DBNull.Value;
            }

            var elementTypeParam = command.Parameters.AddWithValue("elementtype", card.ElementType.ToString());
            elementTypeParam.DataTypeName = "elementtype"; // Name des benutzerdefinierten Enum-Typs in der Datenbank
        }

        private static void AddMonsterCardParameters(NpgsqlCommand command, Card card)
        {
            command.Parameters.AddWithValue("id", card.Id);
        }

        private static void AddSpellCardParameters(NpgsqlCommand command, Card card)
        {
            command.Parameters.AddWithValue("id", card.Id);
        }

    }
}