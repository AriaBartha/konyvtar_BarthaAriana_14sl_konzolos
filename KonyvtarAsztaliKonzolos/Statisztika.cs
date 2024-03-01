using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using KonyvtarAsztaliKonzolos;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

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
        internal static void leghosszabb()
        {
            var longest = books.OrderByDescending(b => b.Page_count).FirstOrDefault();
            Console.WriteLine($"A leghosszabb könyv: \n" +
                                $"\tSzerző: {longest.Author} \n" +
                                $"\tCím: {longest.Title} \n" +
                                $"\tKiadás éve: {longest.Publish_year} \n" +
                                $"\tOldalszám: {longest.Page_count}");
        }
        internal static void legtobbKonyvSzerzoje()
        {
            //SELECT author, COUNT(*) AS book_count FROM books GROUP BY author ORDER BY book_count DESC; 
            //a legtöbb könyvvel rendelkezők 3-an vannak, mindhármuknak 8 könyve van a listában:
            //Kyla Kertzmann III, Briana Kihn és Asha Kreiger

            var result = books.GroupBy(b => b.Author).Select(g => new
                {
                    Author = g.Key,
                    BookCount = g.Count()
                }).OrderByDescending(g => g.BookCount).FirstOrDefault();
            Console.WriteLine($"A legtöbb könyvvel rendelkező szerző: {result.Author}");
        }

        internal static void kiASzerzo()
        {
            Console.Write("Adjon meg egy könyv címet: ");
            string cim = Console.ReadLine();
            var egyezoKonyv = books.Where(book => book.Title == cim).ToList();
            if (egyezoKonyv.Any())
            {
                Console.WriteLine($"A megdott könyv szerzője: {egyezoKonyv[0].Author}");
            }
            else
            {
                Console.WriteLine($"Nincs ilyen könyv");
            }
        }
    }    
}
