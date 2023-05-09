using System;
using System.Data.SqlClient;
using System.Linq;

namespace CreditCardApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=SQL8005.site4now.net;Initial Catalog=db_a98e60_bmtmdt;User Id=db_a98e60_bmtmdt_admin;Password=@Baonhi050502;Encrypt=False";
            SqlConnection connection = new SqlConnection(connectionString);

            Console.Write("Enter Credit Card Number: ");
            string creditCardNumber = Console.ReadLine();

            Console.Write("Enter CSV: ");
            string csv = Console.ReadLine();

            Console.Write("Enter Expiration Date (MM-YY): ");
            string expirationDate = Console.ReadLine();

            string idToken = "";
            string csvToken = "";

            // Generate unique IDToken and CSVToken
            bool isIdTokenUnique = false;
            bool isCsvTokenUnique = false;

            while (!isIdTokenUnique || !isCsvTokenUnique)
            {
                idToken = GenerateRandomString(16);
                csvToken = GenerateRandomString(12);

                connection.Open();

                // Check if IDToken already exists in database
                SqlCommand idTokenCommand = new SqlCommand("SELECT COUNT(*) FROM CreditCardTable WHERE IDToken=@idToken", connection);
                idTokenCommand.Parameters.AddWithValue("@idToken", idToken);

                int idTokenCount = (int)idTokenCommand.ExecuteScalar();
                if (idTokenCount == 0)
                {
                    isIdTokenUnique = true;
                }

                // Check if CSVToken already exists in database
                SqlCommand csvTokenCommand = new SqlCommand("SELECT COUNT(*) FROM CreditCardTable WHERE CSVToken=@csvToken", connection);
                csvTokenCommand.Parameters.AddWithValue("@csvToken", csvToken);

                int csvTokenCount = (int)csvTokenCommand.ExecuteScalar();
                if (csvTokenCount == 0)
                {
                    isCsvTokenUnique = true;
                }

                connection.Close();
            }

            // Insert new credit card record
            SqlCommand insertCommand = new SqlCommand("INSERT INTO CreditCardTable (CreditCardNumber, CSV, ExpirationDate, IDToken, CSVToken) VALUES (@creditCardNumber, @csv, @expirationDate, @idToken, @csvToken)", connection);
            insertCommand.Parameters.AddWithValue("@creditCardNumber", creditCardNumber);
            insertCommand.Parameters.AddWithValue("@csv", csv);
            insertCommand.Parameters.AddWithValue("@expirationDate", expirationDate);
            insertCommand.Parameters.AddWithValue("@idToken", idToken);
            insertCommand.Parameters.AddWithValue("@csvToken", csvToken);

            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Credit Card record has been added successfully!");
        }

        // Helper function to generate random string with specified length
        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
