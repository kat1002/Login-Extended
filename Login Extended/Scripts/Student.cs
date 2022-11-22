using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Extended
{
    public class Student : User
    {
        public void Message(User _user)
        {
            FileStream fs = new FileStream(_path._message, FileMode.Append, FileAccess.Write, FileShare.None);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                Console.Write("Your message: ");
                sw.WriteLine(_user.Name + ": " + Console.ReadLine());
            }
        }

        public void ReadMessage()
        {
            FileStream fs = new FileStream(_path._message, FileMode.Open, FileAccess.Read, FileShare.None);
            using (StreamReader sr = new StreamReader(fs))
            {
                string _line = string.Empty;
                while ((_line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(_line);
                }
            }
        }
    }
}
