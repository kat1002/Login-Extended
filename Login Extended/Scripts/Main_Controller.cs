using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using Login_Extended;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;

namespace Login_Extended
{
    public class Main_Controller
    {
        Path _Path = new Path();

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

        #region Input
        private bool checkInput(string _option)
        {
            if (_option.Length > 2) return false;
            if (_option == "") return false;
            if (_option[0] < '1' || _option[0] > '3') return false;
            return true;
        }
        public void chooseOption(ref string _option)
        {
            _option = "";
            _option = Console.ReadLine();
            while (!checkInput(_option))
            {
                Console.WriteLine("Invalid option! Please choose again!");
                Console.Write("Choose your option: ");
                _option = Console.ReadLine();
            }

        }

        #endregion

        #region Read and Write Method
        public void WriteLine(string _path, string _line)
        {

            FileStream fs = new FileStream(_path, FileMode.Append, FileAccess.Write, FileShare.None);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(_line);
            }

        }

        public bool checkExistingUsername(string _username)
        {

            FileStream fs = new FileStream(_Path._accountPath, FileMode.Open, FileAccess.Read, FileShare.None);
            string _line, _Username;
            using (StreamReader sr = new StreamReader(fs))
            {
                while ((_line = sr.ReadLine()) != null)
                {
                    _Username = _line.Substring(_line.IndexOf("=") + 1, _line.IndexOf(" ") - _line.IndexOf("=")-1);
                    if (_username == _Username) return true;
                }
            }
            return false;
        }
        private bool checkExistingId(string _path, string _id)
        {
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.None);
            string _line, _Id;
            using (StreamReader sr = new StreamReader(fs))
            {
                while ((_line = sr.ReadLine()) != null)
                {
                    _Id = _line.Substring(0, _line.IndexOf("="));
                    if (_Id == _id) return true;
                }
            }
            return false;
        }

        private void GetUser(ref User _user)
        {
            string access = _user.Id.Substring(0,2);
            string _path = "";

            if (access == "AD") return;
            if (access == "HS") _path = _Path._studentInfo;
            else if (access == "TE") _path = _Path._teacherInfo;

            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.None);
           
            string _line, _Id;
            using (StreamReader sr = new StreamReader(fs))
            {
                while ((_line = sr.ReadLine()) != null)
                {
                    _Id = _line.Substring(0, _line.IndexOf("|"));
                    if (_Id == _user.Id)
                    {
                        int[] pos = new int[10];
                        findAll(_line, ref pos);
                        _user.Name = _line.Substring(pos[0] + 1, pos[1] - 1 - pos[0]);
                        _user.Age = Convert.ToInt32(_line.Substring(pos[1] + 1, pos[2] - pos[1] - 1));
                        _user.PhoneNumber = _line.Substring(pos[2] + 1, _line.Length - pos[2] - 1);
                        break;
                    }
                }
            }
        }

        private bool signin(string _path, string _username, string _password, ref User _user)
        {
            FileStream fs = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            string _line, _Username, _Password;
            using (StreamReader sr = new StreamReader(fs))
            {
                while ((_line = sr.ReadLine()) != null)
                {
                    int pos1 = _line.IndexOf("=");
                    int pos2 = _line.IndexOf(" ");
                    _Username = _line.Substring(pos1 + 1, pos2-pos1-1);
                    _Password = _line.Substring(pos2 + 1, _line.Length - pos2 - 1);
                    if (_username == _Username && _password == _Password)
                    {
                        _user.Id = _line.Substring(0, pos1);
                        return true;
                    }
                }
            }
            return false;
        }


        #endregion

        #region Sign-up Option
        public bool checkUsername(string _username)
        {
            if (_username == "") return false;
            foreach (char _char in _username)
            {
                if ((_char >= '0' && _char <= '9') || (_char >= 'A' && _char <= 'Z') || (_char >= 'a' && _char <= 'z') || (_char == '_')) continue;
                return false;
            }

            return true;
        }

        public bool checkPassword(string _password)
        {
            if (_password == "") return false;
            if (_password.Contains(" ")) return false;
            return true;
        }

        public string enterPassword()
        {
        passwordEnter:
            var pass = string.Empty;

            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            ConsoleKey key;

            do
            {
                keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass.Remove(pass.Length - 1, 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();

            if (pass == "")
            {
                Console.Write("Invalid password! Please enter again: ");
                goto passwordEnter;
            }
            return pass;
        }

        public bool checkPassword(string _password, string _confirmPass)
        {
            return _password == _confirmPass;
        }

        public void CreateId(ref User _user)
        {
            CreateId:
            string _id = _user.Id;
            Random rnd = new Random();
            _id += rnd.Next(100000, 999999).ToString();
            if (checkExistingId(_Path._accountPath, _id)) 
            {
                goto CreateId;
            }
            _user.Id = _id;
        }

        public void enterInformation(ref User _user)
        {
            Console.WriteLine("Enter your name: ");
            _user.Name = Console.ReadLine();

            Console.WriteLine("Enter your age: ");
            _user.Age = Convert.ToInt32(Console.ReadLine());

            PhoneNumber:
            Console.WriteLine("Enter your phone number: ");
            _user.PhoneNumber = Console.ReadLine();
            foreach (var item in _user.PhoneNumber)
            {
                if(item < '0' || item > '9')
                {
                    Console.WriteLine("Invalid Phone Number! Please try again!");
                    goto PhoneNumber;
                }
            }

            string _userInfo = _user.Id + "|" + _user.Name + "|" + _user.Age + "|" + _user.PhoneNumber;
            if (_user.Id.Substring(0, 2) == "HS") WriteLine(_Path._studentInfo, _userInfo);
            else if (_user.Id.Substring(0, 2) == "TE") WriteLine(_Path._teacherInfo, _userInfo);
        }

        public void sign_up(ref User _user)
        {
            string _username = "", _id = "", _password = "", _confirmPass = "", _account = "", _option = "";

            Console.WriteLine("1.Teacher Account\n2.Student Account");
            Console.Write("Choose your option: ");
        chooseOption:
            _option = Console.ReadLine();

            switch (_option)
            {
                case "1":
                    _user.Id += "TE";
                    break;

                case "2":
                    _user.Id += "HS";
                    break;

                default:
                    Console.Write("Invalid option! Please choose again: ");
                    goto chooseOption;
            }

        UsernameAssign:
            Console.Write("Enter your username: ");
            _username = Console.ReadLine();
            if (!checkUsername(_username))
            {
                Console.WriteLine("Invalid Character! Please try again!");
                goto UsernameAssign;
            }
            if (checkExistingUsername(_username))
            {
                Console.WriteLine("This username has been taken!");
                goto UsernameAssign;
            }

        PasswordAssign:
            _password = "";
            _confirmPass = "";
            Console.Write("Enter your password: ");
            _password = enterPassword();

            Console.Write("Confirm your password: ");
            _confirmPass = enterPassword();

            if (!checkPassword(_password, _confirmPass))
            {
                Console.WriteLine("Password doesn't match! Please try again!");
                goto PasswordAssign;
            }

            CreateId(ref _user);
            _account = _user.Id + "=" + _username + " " + _password;

            WriteLine(_Path._accountPath,_account);

            enterInformation(ref _user);

            Console.WriteLine("Account created!");
        }

        #endregion

        #region Sign-in Option
        public bool sign_in(string _path, ref string _accessType, ref User _u)
        {
        StartSignin:
            string _username = "", _password = "";
            Console.Write("Enter your username: ");
            _username = Console.ReadLine();

            Console.Write("Enter your password: ");
            _password = enterPassword();

            if (signin(_path, _username, _password, ref _u))
            {
                GetUser(ref _u);
                string access = _u.Id.Substring(0, 2);
                switch (access)
                {
                    case "AD":
                        _accessType = "ADMIN";
                        break;

                    case "TE":
                        _accessType = "TEACHER";
                        break;

                    case "HS":
                        _accessType = "STUDENT";
                        break;
                }
                Console.WriteLine("Signed in as " + _accessType + "!");
                return true;
            }
            else
            {
                string _option = "";
                if (!checkExistingUsername(_username))
                {
                    Console.Write("Doesnt find this username in the database! Do you want to sign up? (Y/N) ");

                chooseOption:
                    _option = Console.ReadLine();
                    _option.Trim(); _option = _option.ToLower();
                    if (_option == "y" || _option == "yes")
                    {
                        Console.Clear();
                        sign_up(ref _u);
                    }
                    else if (_option == "n" || _option == "no") System.Environment.Exit(0);
                    else
                    {
                        Console.Write("Invalid option! Please choose again: ");
                        goto chooseOption;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong password!");
                    goto StartSignin;
                }
            }
            return false;
        }
        #endregion
    }
}
