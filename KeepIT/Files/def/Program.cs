using System;
using System.Threading;
using static KeepIT.Files.Menu.MainMenu;

namespace KeepIT.Files.def
{
    class Program
    {
        /*
         * The application was made by T0m.
         * This Password sotrage aka manager is very simple.
         * Don't forget to exit from the program after using because the key can be pulled from the memory of the application.
        */

        static void Main()
        {
            Console.Title = "KeepIT";

            //For the global error
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalErrorHandler);
            Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(GlobalErrorHandler);

            LoginMenu();

            Console.ReadLine();
        }

        private static void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Console.WriteLine("A global error has occurred: " + ex.Message);
        }


    }
}
