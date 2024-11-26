using System.Text;

namespace MonsterTradingCardsGame.Helper.Database
{
    public static class QueryBuilder
    {
        public static string BuildInsertQuery(string tableName, IEnumerable<string> columns)
        {
            var columnsList = string.Join(", ", columns);
            var parametersList = string.Join(", ", columns.Select(c => "@" + c));
            return $"INSERT INTO {tableName} ({columnsList}) VALUES ({parametersList})";
        }

        public static string BuildUpdateQuery(string tableName, IEnumerable<string> columns, string keyColumn)
        {
            var setClause = string.Join(", ", columns.Select(c => $"{c} = @{c}"));
            return $"UPDATE {tableName} SET {setClause} WHERE {keyColumn} = @{keyColumn}";
        }

        public static string BuildDeleteQuery(string tableName, string keyColumn)
        {
            return $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyColumn}";
        }

        public static string BuildSelectQuery(string tableName, IEnumerable<string> columns, string keyColumn = null)
        {
            var columnsList = string.Join(", ", columns);
            var query = new StringBuilder($"SELECT {columnsList} FROM {tableName}");
            if (!string.IsNullOrEmpty(keyColumn))
            {
                query.Append($" WHERE {keyColumn} = @{keyColumn}");
            }
            return query.ToString();
        }
    }
}