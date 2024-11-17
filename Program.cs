using BasicLibrarysystems.Repositories;
using Microsoft.EntityFrameworkCore;


namespace BasicLibrarysystems
{
    public class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer("Data Source=(local); Initial Catalog=LibrarySystem; Integrated Security=true; TrustServerCertificate=True");

            using (var context = new LibraryContext(optionsBuilder.Options))
            {
                context.Database.Migrate();
            }

            using (var context = new LibraryContext())
            {
                var adminRepo = new AdminRepository(context);
                var userRepo = new UserRepository(context);
                var bookRepo = new BookRepository(context);
                var borrowingRepo = new BorrowingRepository(context);

                Console.WriteLine("Welcome to the Library Management System");
                Console.WriteLine("1. Admin Login");
                Console.WriteLine("2. User Login");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    AdminLogin(adminRepo, bookRepo, userRepo);
                }
                else if (choice == "2")
                {
                    UserLogin(userRepo, borrowingRepo);
                }
            }
        }

        static void AdminLogin(AdminRepository adminRepo, BookRepository bookRepo, UserRepository userRepo)
        {
            Console.Write("Enter Email: ");
            var email = Console.ReadLine();
            Console.Write("Enter Password: ");
            var password = Console.ReadLine();

            var admin = adminRepo.GetByEmail(email);
            if (admin != null && admin.Password == password)
            {
                Console.WriteLine("Admin logged in successfully.");
               
            }
            else
            {
                Console.WriteLine("Invalid credentials.");
            }
        }

        static void UserLogin(UserRepository userRepo, BorrowingRepository borrowingRepo)
        {
            Console.Write("Enter Passcode: ");
            var passcode = Console.ReadLine();

            var user = userRepo.GetAll().FirstOrDefault(u => u.Passcode == passcode);
            if (user != null)
            {
                Console.WriteLine("User  logged in successfully.");
               
            }
            else
            {
                Console.WriteLine("Invalid passcode.");
            }
        }
    }
}
