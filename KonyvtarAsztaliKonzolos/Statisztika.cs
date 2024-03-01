using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace KonyvtarAsztaliKonzolos
{
    internal class Statisztika
    {
        static List<Book> books = new List<Book>();
        static MySqlConnection conn = null;
        static MySqlCommand cmd = null;


        public static void beolvas()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Clear();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "";
            sb.Database = "books";
            sb.CharacterSet = "utf8";
            conn = new MySqlConnection(sb.ConnectionString);
            cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = "SELECT * FROM `books`";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string title = reader.GetString("title");
                        string author = reader.GetString("author");
                        int publish_year = reader.GetInt32("publish_year");
                        int page_count = reader.GetInt32("page_count");
                        books.Add(new Book(id, title, author, publish_year, page_count));
                    }
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
        internal static void konyvListazas()
        {
            foreach (var item in books)
            {
                Console.WriteLine(item);
            }
        }
    }

    
}
