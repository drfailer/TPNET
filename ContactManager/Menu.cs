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

        // help
        private void DisplayHelpMenu()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------- Contact Manager -----------------------------------");
            Console.WriteLine("ls                               - list folder content.");
            Console.WriteLine("tree                             - list all the hierarchy.");
            Console.WriteLine("cd <folder_name>                 - change folder.");
            Console.WriteLine("up                               - go in the parent folder.");
            Console.WriteLine("mkdir <folder_name>              - new folder folder.");
            Console.WriteLine("new <contact info>               - add new contact (interative if no parameters given).");
            Console.WriteLine("edit <name> | <name> <firstName> - edit a contact or a folder.");
            Console.WriteLine("rm <name>|<name> <firstname>     - remove a contact or a folder.");
            Console.WriteLine("save <file_name>                 - save contact in a file (contacts.xml by default).");
            Console.WriteLine("load <file_name>                 - load contact from a file (contacts.xml by default).");
            Console.WriteLine("clear                            - clear the screen.");
            Console.WriteLine("quit | exit                      - quitter.");
            Console.WriteLine("help                             - show this message.");
        }

        // raccourcis pour execution de fonctions à 1 argument (avec message d'erreur si besoin)
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

        // sauvegarde de fichier (le type de fichier à utiliser est derterminé dans le manager via son extention)
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

        // chargement de fichier (le type de fichier à utiliser est derterminé dans le manager via l'extention)
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

        // création d'un nouveau contact (inline ou interactif)
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

        // supression d'un contact ou d'un fichier
        private void RunRm(string[] command)
        {
            if (command.Length == 2)
            {
                manager.RemoveElement(command[1]);
            }
            else if (command.Length == 3)
            {
                manager.RemoveElement(command[1], command[2]);
            }
            else
            {
                Console.WriteLine("Error: you must specify a contact or a folder to remove.");
            }
        }

        // edition d'un contact ou d'un fichier
        private void RunEdit(string[] command)
        {
            if (command.Length == 2)
            {
                manager.EditFolder(command[1]);
            }
            else if (command.Length == 3)
            {
                manager.EditContact(command[1], command[2]);
            }
        }

        // fonction pricipale (fait tourner l'appli)
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
                        RunEdit(command);
                        break;
                    case "rm":
                        RunRm(command);
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
