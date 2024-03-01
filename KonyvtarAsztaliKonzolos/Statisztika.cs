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


        internal static void beolvas()
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

        internal static void otszaznalHosszabb()
        {
            //--todo: SELECT COUNT(`id`) FROM books WHERE page_count > 500;
            var bookCount = books.Where(b => b.Page_count > 500).Count();
            Console.WriteLine($"500 oldalnál hosszabb könyvek száma: {bookCount}");
        }
        internal static void regiKonyv()
        {
            bool old = books.Any(b => b.Publish_year < 1950);
            if (old == true)
            {
                Console.WriteLine("Van 1950-nél régebbi könyv.");
            }
            else
            {
                Console.WriteLine("Nincs 1950-nél régebbi könyv.");
            }
        }



    }    
}
