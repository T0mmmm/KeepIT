using System;
using System.IO;
using static KeepIT.Files.def.Vars;
using static KeepIT.Files.Menu.PasswordStorage;

namespace KeepIT.Files.Menu
{
    class MainMenu
    {
        public static void LoginMenu()
        {
            //For the secret key
            back:
            Console.Clear();
            Console.WriteLine("The key is the 'secret phrase' for your encryption or decryption.");
            Console.WriteLine("Make sure that the key are 15 characters minimum that includes letters, symbols and numbers.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("If you forgot the secret phrase you be not able to get the data back.\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Enter your key.\n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("$: ");
            Console.ForegroundColor = ConsoleColor.White;

            SecretKey = Console.ReadLine();

            //simple check
            if (SecretKey.Length < 15)
            {
                goto back;
            }

            LogoMenu();
        }

        public static void LogoMenu()
        {
            Console.Clear();
            Console.WriteLine(@"db   dD   d88888b   d88888b   d8888b.   d888888b   d888888b ");
            Console.WriteLine(@"88 ,8P'   88'       88'       88  `8D     `88'     `~~88~~' ");
            Console.WriteLine(@"88,8P     88ooooo   88ooooo   88oodD'      88         88 ");
            Console.WriteLine(@"88`8b     88~~~~~   88~~~~~   88~~~        88         88 ");
            Console.WriteLine(@"88 `88.   88.       88.       88          .88.        88 ");
            Console.WriteLine("YP   YD   Y88888P   Y88888P   88        Y888888P      YP  \n\n\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Use the command '-help' to get more information.");
            Console.ForegroundColor = ConsoleColor.White;

            if (SetUP == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Don't forget to setup your path for json file in the settings!\n");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("$: ");
            Console.ForegroundColor = ConsoleColor.White;

            Selector();
        }

        private static void SettingsMenu()
        {
            back:
            Console.Clear();

            Console.WriteLine("1. Change the secret key");
            Console.WriteLine("2. Change or setup the path of json file.");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n$: ");
            Console.ForegroundColor = ConsoleColor.White;

            switch (Console.ReadLine())
            {
                case "1":
                    LoginMenu();
                    break;
                case "2":
                    EnterPath:
                    Console.Clear();
                    Console.WriteLine("Enter the path to json file.");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n$: ");
                    Console.ForegroundColor = ConsoleColor.White;

                    PathPasswords = Console.ReadLine();

                    FileInfo fi = new FileInfo(PathPasswords);

                    if (File.Exists(PathPasswords) && fi.Extension == ".json")
                    {
                        Console.Clear();
                        Console.WriteLine($"{PathPasswords} was successfully saved!");
                        SetUP = true;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\n$: ");
                        Console.ForegroundColor = ConsoleColor.White;

                        Selector();
                    }
                    goto EnterPath;
                default:
                    goto back;
            }
        }

        private static void Selector()
        {
            back:
            switch (Console.ReadLine().ToLower())
            {
                case "pwd":
                    Console.Clear();
                    if (SetUP == false)
                    { LogoMenu(); }
                    PasswordStorageMenu();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                case "-help":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Commands           Desorption");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("--------           ----------");
                    Console.WriteLine("pwd                Open the options of password storage.");
                    Console.WriteLine("exit               Close the application.");
                    Console.WriteLine("back               You can go to the main menu.");
                    Console.WriteLine("about              Some information about the app.");
                    Console.WriteLine("settings           Open the settings menu.\n");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("$: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    goto back;

                case "back":
                    LogoMenu();
                    break;
                case "settings":
                    SettingsMenu();
                    break;
                case "about":
                    Console.Clear();
                    Console.WriteLine("Simple password keeper with very simple encrypt and decrypt data.\nCreated by T0m.");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n$: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    goto back;
                default:
                    LogoMenu();
                    break;
            }
        }
    }
}
