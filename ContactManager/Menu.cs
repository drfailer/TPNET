using HierarchicalStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager
{
    public class Menu
    {
        private delegate void CMOperation(string name);
        Manager manager;

        private void DisplayHelpMenu()
        {
            Console.Clear();
            Console.WriteLine("---------------------------- Contact Manager ----------------------------");
            Console.WriteLine("ls                  - list folder content.");
            Console.WriteLine("tree                - list all the hierarchy.");
            Console.WriteLine("cd <folder_name>    - change folder.");
            Console.WriteLine("up                  - go in the parent folder.");
            Console.WriteLine("mkdir <folder_name> - new folder folder.");
            Console.WriteLine("new <contact info>  - add new contact (interative if no parameters given).");
            Console.WriteLine("edit <name>         - edit a contact or a folder.");
            Console.WriteLine("rm <name>           - remove a contact or a folder.");
            Console.WriteLine("save <file_name>    - save contact in a file (contacts.xml by default).");
            Console.WriteLine("load <file_name>    - load contact from a file (contacts.xml by default).");
            Console.WriteLine("clear               - clear the screen.");
            Console.WriteLine("quit | exit         - quitter.");
            Console.WriteLine("help                - show this message.");
        }

        private void RunCommand(string[] command, CMOperation operation, string errorMessage)
        {
            if (command.Length == 2)
            {
                operation(command[1]);
            }
            else
            {
                Console.WriteLine(errorMessage);
            }
        }

        private void RunSave(string[] command)
        {
            Console.Write("key: ");
            string key = Console.ReadLine();
            if (command.Length == 2)
            {
                manager.SaveFile(command[1], key);
            }
            else
            {
                manager.SaveFile(key);
            }
        }

        private void RunLoad(string[] command)
        {
            Console.Write("key: ");
            string key = Console.ReadLine();
            if (command.Length == 2)
            {
                manager.LoadFile(command[1], key);
            }
            else
            {
                Console.WriteLine("No file secified, a default file will be chosen !");
                manager.LoadFile(key);
            }
        }

        private void RunNew(string[] command)
        {
            string name;
            string firstname;
            string mail;
            string company;
            string link;

            if (command.Length == 1)
            { // mode interactif
                Console.Write("name: ");
                name = Console.ReadLine();

                Console.Write("firstname: ");
                firstname = Console.ReadLine();

                Console.Write("mail: ");
                mail = Console.ReadLine();

                Console.Write("company: ");
                company = Console.ReadLine();

                Console.Write("link: ");
                link = Console.ReadLine();

                manager.AddContact(name, firstname, mail, company, link);
            }
            else if (command.Length == 6)
            {
                name = command[1];
                firstname = command[2];
                mail = command[3];
                company = command[4];
                link = command[5];
                manager.AddContact(name, firstname, mail, company, link);
            }
            else
            {
                Console.WriteLine("Error: require 6 arguments arguments:");
                Console.WriteLine("\tnew name firstName mail comapany LINK");
            }
        }

        public void Run()
        {
            manager = new Manager();
            string[] command;
            bool run = true;

            while (run)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("cm> ");
                Console.ForegroundColor = ConsoleColor.Gray;
                command = Console.ReadLine().Split(' ');

                switch (command[0])
                {
                    case "ls":
                        manager.ListCurrentFolder();
                        break;
                    case "tree":
                        manager.Display();
                        break;
                    case "cd":
                        RunCommand(command, manager.GoDown, "Error: you must specify a correct folder name");
                        break;
                    case "up":
                        manager.GoUp();
                        break;
                    case "mkdir":
                        RunCommand(command, manager.AddFolder, "Error: you must specify a correct folder name");
                        break;
                    case "new":
                        RunNew(command);
                        break;
                    case "edit":
                        RunCommand(command, manager.EditContact, "Error: you must specify the name of an existing contact.");
                        break;
                    case "rm":
                        RunCommand(command, manager.RemoveElement, "Error: you must specify the name of an existing contact.");
                        break;
                    case "save":
                        RunSave(command);
                        break;
                    case "load":
                        RunLoad(command);
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "exit":
                    case "quit":
                        run = false;
                        break;
                    case "help":
                        DisplayHelpMenu();
                        break;
                    default:
                        Console.WriteLine("unknown command");
                        break;
                }
            }
        }
    }
}
