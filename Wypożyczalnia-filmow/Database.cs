using Npgsql;
using System.IO;

namespace Wypożyczalnia_filmow
{
    public static class Database
    {
        private const string Host = "100.80.77.35";    // np. localhost lub adres IP
        private const string Port = "5432";
        private const string DbName = "movie_rental";   // np. wypozyczalnia
        private const string User = "appuser";   // np. postgres

        // Plik haslo.txt musi znajdować się w tym samym folderze co .exe (bin\Debug\...)
        private static readonly string PasswordFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haslo.txt");

        private static string? _connectionString;

        private static string ConnectionString
        {
            get
            {
                if (_connectionString != null) return _connectionString;

                if (!File.Exists(PasswordFilePath))
                    throw new FileNotFoundException($"Nie znaleziono pliku z hasłem: {PasswordFilePath}");

                string password = File.ReadAllText(PasswordFilePath).Trim();

                _connectionString = $"Host={Host};Port={Port};Database={DbName};Username={User};Password={password};Timeout=30;CommandTimeout=60;SSL Mode=Disable;";
                return _connectionString;
            }
        }

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}