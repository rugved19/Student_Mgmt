using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Mgmt
{
    class Program
    {
        private static SqlConnection _connection;

        static void Main(string[] args)
        {
            _connection = new SqlConnection("Server=localhost;Database=StudentMgmt;User Id=RUGVED19\\RUGVED;Password=Rugved@1903;");

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Student Management System");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Edit Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Search Students");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        ViewStudents();
                        break;
                    case "3":
                        EditStudent();
                        break;
                    case "4":
                        DeleteStudent();
                        break;
                    case "5":
                        SearchStudents();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void AddStudent()
        {
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter Age: ");
            string ageInput = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            if (!ValidateInputs(firstName, lastName, ageInput, email, out int age))
            {
                Console.WriteLine("Invalid input. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                _connection.Open();
                string query = "INSERT INTO Students (FirstName, LastName, Age, Email) VALUES (@FirstName, @LastName, @Age, @Email)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Email", email);
                command.ExecuteNonQuery();

                Console.WriteLine("Student added successfully! Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
                Console.ReadKey();
            }
        }

        private static void ViewStudents()
        {
            try
            {
                _connection.Open();
                string query = "SELECT * FROM Students";
                SqlCommand command = new SqlCommand(query, _connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("\nID\tFirst Name\tLast Name\tAge\tEmail");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["StudentID"]}\t{reader["FirstName"]}\t{reader["LastName"]}\t{reader["Age"]}\t{reader["Email"]}");
                }

                reader.Close();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                _connection.Close();
            }
        }

        private static void EditStudent()
        {
            Console.Write("Enter Student ID to Edit: ");
            string idInput = Console.ReadLine();
            if (!int.TryParse(idInput, out int studentId))
            {
                Console.WriteLine("Invalid Student ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter New First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter New Last Name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter New Age: ");
            string ageInput = Console.ReadLine();
            Console.Write("Enter New Email: ");
            string email = Console.ReadLine();

            if (!ValidateInputs(firstName, lastName, ageInput, email, out int age))
            {
                Console.WriteLine("Invalid input. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                _connection.Open();
                string query = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, Age = @Age, Email = @Email WHERE StudentID = @StudentID";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@StudentID", studentId);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Email", email);
                command.ExecuteNonQuery();

                Console.WriteLine("Student updated successfully! Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                _connection.Close();
            }
        }

        private static void DeleteStudent()
        {
            Console.Write("Enter Student ID to Delete: ");
            string idInput = Console.ReadLine();
            if (!int.TryParse(idInput, out int studentId))
            {
                Console.WriteLine("Invalid Student ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                _connection.Open();
                string query = "DELETE FROM Students WHERE StudentID = @StudentID";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@StudentID", studentId);
                command.ExecuteNonQuery();

                Console.WriteLine("Student deleted successfully! Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                _connection.Close();
            }
        }

        private static void SearchStudents()
        {
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine();

            try
            {
                _connection.Open();
                string query = "SELECT * FROM Students WHERE FirstName LIKE @SearchTerm OR LastName LIKE @SearchTerm";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("\nID\tFirst Name\tLast Name\tAge\tEmail");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["StudentID"]}\t{reader["FirstName"]}\t{reader["LastName"]}\t{reader["Age"]}\t{reader["Email"]}");
                }

                reader.Close();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                _connection.Close();
            }
        }

        private static bool ValidateInputs(string firstName, string lastName, string ageInput, string email, out int age)
        {
            age = 0;

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(ageInput) ||
                string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (!int.TryParse(ageInput, out age))
            {
                return false;
            }

            try
            {
                var emailCheck = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
