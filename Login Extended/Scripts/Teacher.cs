using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Extended
{
    public class Teacher : User
    {
        public void findAll(string _line, ref int[] pos)
        {
            int index = 0, i = 0;
            while (true)
            {
                index = _line.IndexOf("|", index + 1);
                if (index == -1) break;
                pos[i] = index;
                ++i;
            }
        }
        public void ShowStudent()
        {
            FileStream fs = new FileStream(_path._studentInfo, FileMode.Open, FileAccess.Read, FileShare.None);
            using (StreamReader sr = new StreamReader(fs))
            {
                string _userName = "", _userId = "", _line = "", _userPhoneNumber = "", _userAge = "";
                Console.WriteLine("||   {0, -10} || {1,-20} || {2,-4} || {3,-15} ||", "ID", "Name", "Age", "Phone Number");
                while ((_line = sr.ReadLine()) != null)
                {
                    int[] pos = new int[10];
                    findAll(_line, ref pos);
                    _userId = _line.Substring(0, pos[0]);
                    _userName= _line.Substring(pos[0] + 1, pos[1] - 1 - pos[0]);
                    _userAge = _line.Substring(pos[1] + 1, pos[2] - pos[1] - 1);
                    _userPhoneNumber = _line.Substring(pos[2] + 1, _line.Length - pos[2] - 1);
                    Console.WriteLine("||   {0, -10} || {1,-20} || {2,-4} || {3, -15} ||", _userId, _userName, _userAge, _userPhoneNumber);
                }
            }
        }
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

        public void ClearAllMessage()
        {
            using (StreamReader sr = new StreamReader(_path._message))
            {
                using (StreamWriter sw = new StreamWriter(_path._tempFile))
                {
                    sw.WriteLine();
                }
            }

            File.Delete(_path._message);
            File.Move(_path._tempFile, _path._message);
        }

    }
}
