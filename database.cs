using MySql.Data.MySqlClient;

namespace ControleEstoque
{
    public class Database
    {
        // 🔹 Ajuste os dados de acordo com seu MySQL
        private static string connectionString =
            "Server=localhost;Database=estoque;User ID=root;Password=senha;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
