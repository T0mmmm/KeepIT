using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static KeepIT.Files.def.Encryption;
using static KeepIT.Files.def.Vars;
using static KeepIT.Files.Menu.MainMenu;

namespace KeepIT.Files.Menu
{
    class PasswordStorage
    {

        public static void PasswordStorageMenu()
        {
            back:
            Console.Clear();

            //Some options
            Console.WriteLine("Here is some options.");
            Console.WriteLine("1. Add new passwords");
            Console.WriteLine("2. View passwords");
            Console.WriteLine("3. Remove password");
            Console.WriteLine("4. Edit password\n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("$: ");
            Console.ForegroundColor = ConsoleColor.White;

            //Selector
            switch (Console.ReadLine())
            {
                case "1":
                    PasswordsAdd();
                    break;
                case "2":
                    ViewPasswords();
                    break;
                case "3":
                    RemovePassword();
                    break;
                case "4":
                    EditPassword();
                    break;
                default:
                    goto back;
            }
        }

        private static void GoBack()
        {
            //i am lazy 
            if (Console.ReadLine().ToLower() == "back")
            {
                Console.Clear();
                LogoMenu();
            }
            else { LogoMenu(); }

        }

        private static void EditPassword()
        {
            try
            {
                //parameters
                int id = 0;
                string newPassword = "";
                string newEamil = "";
                back:

                Console.Clear();

                //simple check if the file have any text (data)
                if (File.ReadAllText(PathPasswords).Length == 0)
                {
                    goto back;
                }

                // read the json file
                string readjson = File.ReadAllText(PathPasswords);
                // deserialize the json data
                List<dynamic> data = JsonConvert.DeserializeObject<List<dynamic>>(readjson);

                Console.WriteLine("Enter the id of the object that you want edit.");
                Console.WriteLine("If you don't want to edit some of the parameters leave it blank.");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n$: ");
                Console.ForegroundColor = ConsoleColor.White;

                try
                {
                    id = Int32.Parse(Console.ReadLine());
                }
                catch { }

                //find the id
                if (data.FirstOrDefault(x => x.ids == id) == null)
                {
                    goto back;
                }

                Console.Clear();

                Console.Write("Enter new email: ");

                newEamil = Encrypt(Console.ReadLine(), SecretKey);

                Console.Write("Enter new password: ");

                newPassword = Encrypt(Console.ReadLine(), SecretKey);

                Console.Clear();

                // loop through the data to find the item with the specified id
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].ids == id)
                    {
                        //update the password, email for the item
                        //encrypt use because it is encrypt the null data and it is not become null
                        if (newEamil != Encrypt("", SecretKey))
                        {
                            data[i].Emails = newEamil;
                        }

                        if (newPassword != Encrypt("", SecretKey))
                        {
                            data[i].Passwords = newPassword;
                        }

                        // serialize the updated data
                        string json = JsonConvert.SerializeObject(data);
                        // write the updated data back to the file
                        File.WriteAllText(PathPasswords, json);
                        Console.WriteLine("Password updated successfully!");
                        break;
                    }
                }

                Console.WriteLine("Enter 'back' go to the main menu.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n$: ");
                Console.ForegroundColor = ConsoleColor.White;

                GoBack();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }

        private static void RemovePassword()
        {
            int id = 0;
            back:
            Console.Clear();

            //simple check if the file have any text (data)
            if (File.ReadAllText(PathPasswords).Length == 0)
            {
                goto back;
            }

            Console.Clear();
            Console.WriteLine("Enter the id of the data that you want remove.");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n$: ");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                id = Int32.Parse(Console.ReadLine());
            }
            catch { }

            Console.Clear();

            //read json file
            string json = File.ReadAllText(PathPasswords);
            List<dynamic> data = JsonConvert.DeserializeObject<List<dynamic>>(json);

            //find the object with the id
            var removeObject = data.FirstOrDefault(x => x.ids == id);

            if (removeObject != null)
            {
                data.Remove(removeObject);
                Console.WriteLine("Object with ID " + id + " has been removed.");
            }
            else
            {
                Console.WriteLine("Object with ID " + id + " not found.");
            }

            //saving
            string updatedJson = JsonConvert.SerializeObject(data);
            File.WriteAllText(PathPasswords, updatedJson);

            Console.WriteLine("Enter 'back' go to the main menu.");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n$: ");
            Console.ForegroundColor = ConsoleColor.White;

            GoBack();

        }

        private static void ViewPasswords()
        {
            Console.Clear();

            //simple check
            if (File.ReadAllText(PathPasswords).Length == 0)
            {
                Console.WriteLine("Your json file don't have any data.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n$: ");
                Console.ForegroundColor = ConsoleColor.White;
                GoBack();
            }

            //read the json string from the file
            string json = File.ReadAllText(PathPasswords);

            //deserialize the json string into a list of objects
            List<dynamic> data = JsonConvert.DeserializeObject<List<dynamic>>(json);

            Console.WriteLine("Enter 'back' go to the main menu.");
            Console.WriteLine("Here the decrypted data.\n");

            foreach (var item in data)
            {
                Console.WriteLine("============================================");
                Console.WriteLine("|=> Id: " + item.ids);
                Console.WriteLine("|=> Website: " + Decrypt(item.Websites.ToString(), SecretKey));
                Console.WriteLine("|=> Email: " + Decrypt(item.Emails.ToString(), SecretKey));
                Console.WriteLine("|=> Password: " + Decrypt(item.Passwords.ToString(), SecretKey));
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n$: ");
            Console.ForegroundColor = ConsoleColor.White;

            GoBack();

        }

        private static void PasswordsAdd()
        {

            Console.Clear();

            List<object> data = new List<object>();
            string readjson = File.ReadAllText(PathPasswords);
            List<dynamic> Getdata = JsonConvert.DeserializeObject<List<dynamic>>(readjson);

            //Steps
            Console.WriteLine("Enter data, press enter after each value.");
            Console.WriteLine("Enter 'done' when finished.");
            Console.WriteLine("Leave it blank for add more accounts.\n");

            //Checks if the json file have any data
            if (readjson.Length != 0)
            {
                data = JsonConvert.DeserializeObject<List<object>>(readjson);
                var lastObject = Getdata.LastOrDefault();
                if (lastObject != null)
                {
                    Id = (int)lastObject["ids"];
                }
            }

            //while for add info and repeat the process until type done
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n$: ");
                Console.ForegroundColor = ConsoleColor.White;

                //simple check
                if (Console.ReadLine().ToLower() == "done")
                    break;
                Console.Clear();
                Console.Write("Website: ");
                Website = Console.ReadLine();
                Console.Write("Email: ");
                Email = Console.ReadLine();
                Console.Write("Password: ");
                Password = Console.ReadLine();

                ++Id;

                data.Add(new { Websites = Encrypt(Website, SecretKey), Emails = Encrypt(Email, SecretKey), Passwords = Encrypt(Password, SecretKey), ids = Id });

            }

            //serialize the list of objects to a json string
            string json = JsonConvert.SerializeObject(data);

            Console.Clear();
            //write the json string to a file
            File.WriteAllText(PathPasswords, json); 

            Console.WriteLine($"Data written to {PathPasswords}");
            Console.WriteLine("Enter 'back' go to the main menu.");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n$: ");
            Console.ForegroundColor = ConsoleColor.White;

            GoBack();

        }
    }
}