﻿using System.ComponentModel;
using System.ComponentModel.Design;
using System.Net.Security;
using System.Reflection;
using System.Reflection.Metadata;
using System.Timers;

namespace Atm_CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LandingPage();
            Menu();
            



        }

        public static void Menu()
        {
            Console.Clear();

            Console.WriteLine("Hello! Select a transaction: ");
            Console.WriteLine("1. Withdrawal         2. Balance Enquiry");
            Console.WriteLine("3. Deposit            4. Transfer");
            Console.WriteLine("5. Change Pin         6. Pay a Bill");
            Console.WriteLine("7. Create New Account  0. Exit");

            var input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    Helper.Exit();
                    break;
                case "1":
                    Control.Withdraw();
                    break;
                case "2":
                    //checkBalance();
                    break;
                case "3":
                    //depositMoney();
                        break;
                case "4":
                    //sendMoney();
                    break;
                case "5":
                    //changePin();
                    break;
                case "6":
                    //payBill();
                    break;
                case "7":
                    Db.Create();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    Menu();
                    break;
                    
            }
        }

        public static void LandingPage()
        {
            Response resp;

            Console.Write("Enter your account number: ");
            User user = User.ValidateAccount(Console.ReadLine());


            Console.Clear();
            Console.WriteLine("Hello, {0}\n", user.FullName);

            resp = User.Auth(user);
            //pick it up from here. after inputting wrong pin numerous times, program breaks and logs-in eventually.
            do
                User.Auth(user);
            while (resp == Response.Failed);

            if (resp == Response.Success)
            {
                Console.Clear();
                Console.WriteLine("Login successful");
                
                Thread.Sleep(2000);
                Menu();
            }
            else if (resp == Response.Redirect)
            {
                Helper.Exit();
                //Console.Write("Return to main menu? y/n");
                //var input = Console.ReadLine();
                //if (input.tolower() == "y")
                //{
                //    menu();
                //}
                //else if (input.tolower() == "n")
                //{
                //    user.auth(user);
                //}
            }
            else
            {
                Console.WriteLine("oops an error occurred. Press enter to quit");
                Console.ReadLine();
                Helper.Exit();
            }
        }

    }

    class Control
    {
        public static void Withdraw()
        {
            Console.WriteLine("Enter an amount to withdraw");
            var input = Console.ReadLine();

        }
    }

    class Helper
    {
        public static void countdown(int time) {
            
            for (int i = time; i > 0; i--)
            {
                Console.Clear();
                Console.WriteLine("Redirecting in ({0})", i);
                Thread.Sleep(500);
            }
            Console.Clear();
            Program.LandingPage();
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }
    }

    class User
    {

        public string? FullName { get; set; }
        public string? AccountNo { get; set; }
        public string? Pin { get; set; }
        public Decimal Balance { get; set; }

        public static Response Auth(User user)
        {
            Console.WriteLine("**Enter 0 to cancel**");
            Console.WriteLine("Enter your pin here: ");
            var input = Console.ReadLine();

            if (input == user.Pin)
            {
                return Response.Success;
            } else if (input == "0")
            {
                return Response.Redirect;
            } else
            {
                //Console.WriteLine("Error: In-correct login details");
                return Response.Failed;
            }
        }

        public static User ValidateAccount(string acc)
        {
            string? input;

            User user = new();

            try
            {
                user = Db.users[acc];

            }
            catch
            {
                Console.WriteLine("Error: Check the account number and try again.\n");
                Console.WriteLine("Enter 1 to try again      Enter \"yes\" to create an account\nEnter 0 to exit.\n");

                input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Helper.Exit();
                        break;
                    case "1":
                        Program.LandingPage();
                        break;
                    case "yes":
                        Db.Create();
                        break;
                    default:
                        Console.WriteLine("Error: Invalid Input!");
                        ValidateAccount(acc);
                        break;
                }

                Console.ReadLine();
                Console.WriteLine("Please wait while we re-direct you to the login page...");
                Helper.countdown(3);
            }

            return user;
        }

    }

    class Db
    {
        public static Dictionary<string, User> users = new()
        {
            { "2000", new User { FullName = "AKpabio Sultan", AccountNo = "2000", Pin = "1234" } },
            {"2001", new User {FullName = "Janet Elopolo", AccountNo = "2001", Pin = "1424"} },
            { "2002", new User {FullName = "Denason Albert", AccountNo = "2003", Pin = "0310"} }
        };

        public static void Create()
        {
            Console.Clear();
            Console.WriteLine("**Create a new account**\n");

            User user = new();

            Console.WriteLine("Enter your first and last name: ");
            user.FullName = Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Choose a 4-digit pin you can remember: ");
            user.Pin = Console.ReadLine();

            var keys = users.OrderByDescending(user => users.Keys).Last();
            var accNo = (Int32.Parse(keys.Key) + 1).ToString();
            user.AccountNo = accNo;

            users.Add(accNo, user);

            Console.Clear();
            Console.WriteLine("Account created. Your new account number is {0}", accNo);
            //Console.ReadLine();
        }
    }

    enum Response
    {
        Success = 200,
        Failed = 400,
        Redirect = 202
    }

}