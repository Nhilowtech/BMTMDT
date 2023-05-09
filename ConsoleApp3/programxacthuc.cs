using System;
using System.Data.SqlClient;

namespace ConsoleApp
{
    class programxacthuc
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-3K158E8\\SQLEXPRESS;Database=token;Integrated Security = true";

            Console.Write("Enter ID Token: ");
            string idToken = Console.ReadLine();

            Console.Write("Enter CSV Token: ");
            string csvToken = Console.ReadLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT CreditCardNumber, ExpirationDate, CSV FROM CreditCardTable WHERE IDToken = @IDToken AND CSVToken = @CSVToken", connection);
                    command.Parameters.AddWithValue("@IDToken", idToken);
                    command.Parameters.AddWithValue("@CSVToken", csvToken);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string creditCardNumber = reader.GetString(0);
                        string expirationDate = reader.GetString(1);
                        string csv = reader.GetString(2);

                        Console.WriteLine($"Credit Card Number: {creditCardNumber}");
                        Console.WriteLine($"Expiration Date: {expirationDate}");
                        Console.WriteLine($"CSV: {csv}");
                    }
                    else
                    {
                        Console.WriteLine("Token information is invalid.");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
