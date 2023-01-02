using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager;

namespace TPNET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.Run();
/*            Manager m = new Manager();
            m.AddFolder("test");
            m.AddFolder("school");
            m.AddContact("test", "test", "test.test@gmail.com", "WD", "Friend");
            m.GoDown("school");
            m.AddContact("a", "b", "a.b@gmail.com", "WD", "Network");
            m.GoUp();
            m.GoDown("test");
            m.AddContact("c", "c", "c.c@gmail.com", "WD", "Network");
            m.AddFolder("testintest");
            m.GoDown("testintest");
            m.AddContact("d", "d", "d.d@gmail.com", "WD", "Collegue");

            Console.WriteLine("saving file");
            m.SaveFile("test.dat");

            Manager m2 = new Manager();
            m2.LoadFile("test.dat");
            m2.Display();
*/        }
    }
}
