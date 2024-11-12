using System;
using System.Collections.Generic;


namespace ChubbBank
{
    public class Account
    {
        public string acc_no;
        public string holder;
        public string type;
        public decimal initial_deposit;
        public List<Transaction> accountTransactions = new List<Transaction>();
    }

    public class User
    {
        public string Username;
        public string Password;
        public List<Account> userAccounts = new List<Account>();
    }

    public class Transaction
    {
        public string trans_id;
        public DateTime date;
        public string type;
        public decimal amount;
    }


    class MainClass
    {
        public static List<User> users = new List<User>();  //list of all the registered users
        public static User loggedInUser = new User();  //the user which is currently logged in 

        //function to register user
        public static void RegisterUser()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            //check if username exists
            for(int i=0; i<users.Count; i++)
            {
                if(users[i].Username == username)
                {
                    Console.WriteLine("Username already exists");
                    return;
                }
            }

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            users.Add(new User { Username = username, Password = password });
            Console.WriteLine("User created successfully.");
        }

        //functino to login
        public static void LogIn()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();


            Console.Write("Enter password: ");
            string password = Console.ReadLine();

        
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Username == username)
                {
                    if(password != users[i].Password)
                    {
                        Console.WriteLine("Wrong Password");
                        return;
                    }
                    loggedInUser = users[i];  //makes this the current logged in user
                    Console.WriteLine("Logged in Succesfully");
                    return;
                }
            }

            
            Console.WriteLine("Username doesn't exist");
            return;
            
        }

        //to create an account for a user
        public static void OpenAccount(User user)
        {
            //to check if user is logged in 
            if (user == null || string.IsNullOrEmpty(user.Username))
            {
                Console.WriteLine("Log in first");
                return;
            }

            Console.Write("Enter account holder's name: ");
            string holderName = Console.ReadLine();
            Console.Write("Enter initial deposit amount: ");
            decimal initialDeposit = decimal.Parse(Console.ReadLine());

            string accountNumber = Guid.NewGuid().ToString().Substring(0, 8); 

            Console.WriteLine("Select account type: 1. Savings  2. Checking");
            string accountType = Console.ReadLine() == "1" ? "Savings" : "Checking";

            user.userAccounts.Add(new Account {acc_no=accountNumber, holder=holderName, initial_deposit=initialDeposit, type=accountType });
            Console.WriteLine($"Account {accountNumber} created successfully");
        }

        //to deposit money into an account
        public static void Deposit(User user)
        {
            //to check if user is logged in 
            if (user == null || string.IsNullOrEmpty(user.Username))
            {
                Console.WriteLine("Log in first");
                return;
            }
            Console.Write("Enter account number: ");
            string account_no_input = Console.ReadLine();
            Account account = user.userAccounts.Find(a => a.acc_no == account_no_input);

            //to check if account exists
            if(account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }

            Console.WriteLine("Enter deposit amount: ");
            decimal amt = decimal.Parse(Console.ReadLine());
            account.initial_deposit += amt;

            account.accountTransactions.Add(new Transaction { amount = amt, date = DateTime.Now, type = "Deposit", trans_id = Guid.NewGuid().ToString().Substring(0, 8) });

            Console.WriteLine("Amount deposited succesfully");
        }

        //to deposit money into an account
        public static void Withdraw(User user)
        {
            //to check if user is logged in 
            if (user == null || string.IsNullOrEmpty(user.Username))
            {
                Console.WriteLine("Log in first");
                return;
            }
            Console.Write("Enter account number: ");
            string account_no_input = Console.ReadLine();
            Account account = user.userAccounts.Find(a => a.acc_no == account_no_input);

            //to check if account exists
            if (account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }

            Console.WriteLine("Enter withdraw amount: ");
            decimal amt = decimal.Parse(Console.ReadLine());
            //to check if there is enough balance
            if (account.initial_deposit < amt)
            {
                Console.WriteLine("Insufficient balance");
                return;
            }

            account.initial_deposit -= amt;

            account.accountTransactions.Add(new Transaction { amount = amt, date = DateTime.Now, type = "Withdraw", trans_id = Guid.NewGuid().ToString().Substring(0, 8) });

            Console.WriteLine("Amount deposited succesfully");
        }

        //function to generate statement for an account
        public static void GenerateStatement(User user)
        {
            Console.Write("Enter account number: ");
            string account_no_input = Console.ReadLine();
            Account account = user.userAccounts.Find(a => a.acc_no == account_no_input);

            //to check if account exists
            if (account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }

            //to iterate through all the transactions
            foreach (var transaction in account.accountTransactions)
            {
                Console.WriteLine($"{transaction.date} - {transaction.type}: {transaction.amount}");
            }
        }

        //function to check balance in an account
        public static void balanceCheck(User user)
        {
            Console.Write("Enter account number: ");
            string account_no_input = Console.ReadLine();
            Account account = user.userAccounts.Find(a => a.acc_no == account_no_input);

            //to check if account exists or not
            if (account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }
            Console.WriteLine($"Balance is {account.initial_deposit}");
        }

        //function to calculate interest and add it to the account
        public static void calculateInterest(User user)
        {
            Console.Write("Enter account number: ");
            string account_no_input = Console.ReadLine();
            Account account = user.userAccounts.Find(a => a.acc_no == account_no_input);

            //to check if account exists or not
            if (account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }
            if (account.type != "Savings")
            {
                Console.WriteLine("Not savings account");
                return;
            }
            decimal interest = 0.03m * account.initial_deposit;
            account.initial_deposit += interest;
            Console.WriteLine("Interest added to the account");
        }

        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            while (true)
            {
                Console.WriteLine("\n1. Register\n2. Login\n3. Open account\n4. Deposit\n5. Withdraw\n6. Check Balance\n7. Calculate Interest\n8. Generate statement");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        LogIn();
                        break;
                    case "3":
                        OpenAccount(loggedInUser);
                        break;
                    case "4":
                        Deposit(loggedInUser);
                        break;
                    case "5":
                        Withdraw(loggedInUser);
                        break;
                    case "6":
                        balanceCheck(loggedInUser);
                        break;
                    case "7":
                        calculateInterest(loggedInUser);
                        break;
                    case "8":
                        GenerateStatement(loggedInUser);
                        break;
                    default:
                        Console.WriteLine("Exit..."); //This will exit the loop
                        return;
                }
            }
        }
    }
}
