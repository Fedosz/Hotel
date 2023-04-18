using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace Assistance_Prog.MVVM.Model
{
    internal static class Logger
    {
        static private readonly string rootFolder = @"//Data//staff.txt";
        static private List<Person> people = new List<Person>();

        private struct Person
        {
            public string login;
            public string password;
            public Permission permission;
            public int id;

            public Person(string login, string password, int permission, int id)
            {
                this.login = login;
                this.password = password;
                this.permission =(Permission) permission;
                this.id = id;
            }
        }

        static public User Login(string username, string password)
        {
            User user = new User();
            
            foreach (Person person in people)
            {
                if (person.login == username && person.password == password) 
                {
                    user.Log(person.id, person.permission);
                    break;
                }
            }

            return user;
        }

        static public void LoadData()
        {
            string[] lines;

            if (File.Exists(rootFolder))
            {
                lines = File.ReadAllLines(rootFolder);
            }
            else
            {
                throw new DirectoryNotFoundException("Error logging users");
            }

            foreach (string line in lines)
            {
                string[] data = line.Split(";");
                int id = int.Parse(data[2]);
                int permission = int.Parse(data[3]);
                Person person = new Person(data[0], data[1], permission, id);
            }
        }
    }
}
