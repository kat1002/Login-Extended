using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Login_Extended
{
    public class Admin : Teacher
    {
        Main_Controller ctl = new Main_Controller();
        
        private void ShowAccountFile()
        {
            FileStream fs = new FileStream(_path._accountPath, FileMode.Open, FileAccess.Read, FileShare.None);
            using (StreamReader sr = new StreamReader(fs))
            {
                string _username = "", _userId = "", _line = "";
                Console.WriteLine("||   {0, -10} || {1,-20} ||", "ID", "Username");
                while ((_line = sr.ReadLine()) != null)
                {
                    int pos1 = _line.IndexOf("="), pos2 = _line.IndexOf(" ");
                    _userId = _line.Substring(0, pos1);
                    _username = _line.Substring(pos1 + 1, pos2 - pos1 - 1);

                    Console.WriteLine("||   {0, -10} || {1,-20} ||", _userId, _username);
                }
            }
        }
        private void RemoveSpecificUser()
        {
            string _username;
            Console.WriteLine("Choose user to remove (Enter username): ");
            _username = Console.ReadLine();

            string _tempFile = _path._tempFile;
            using (StreamReader sr = new StreamReader(_path._accountPath))
            {
                using (StreamWriter sw = new StreamWriter(_tempFile))
                {
                    string _line;
                    while((_line = sr.ReadLine()) != null)
                    {
                        if (_line.Substring(_line.IndexOf("=") + 1, _line.IndexOf(" ") - 1 - _line.IndexOf("=")) != _username){
                            sw.WriteLine(_line);
                        }
                    }
                }
            }

            File.Delete(_path._accountPath);
            File.Move(_tempFile, _path._accountPath);
        }
        private void AddAdmin(ref User _user) 
        {
            string _username = "", _password = "", _confirmPass = "", _account = "";
        UsernameAssign:
            Console.Write("Enter your username: ");
            _username = Console.ReadLine();
            if (!ctl.checkUsername(_username))
            {
                Console.WriteLine("Invalid Character! Please try again!");
                goto UsernameAssign;
            }
            if (ctl.checkExistingUsername(_username))
            {
                Console.WriteLine("This username has been taken!");
                goto UsernameAssign;
            }

        PasswordAssign:
            _password = "";
            _confirmPass = "";
            Console.Write("Enter your password: ");
            _password = ctl.enterPassword();

            Console.Write("Confirm your password: ");
            _confirmPass = ctl.enterPassword();

            if (!ctl.checkPassword(_password, _confirmPass))
            {
                Console.WriteLine("Password doesn't match! Please try again!");
                goto PasswordAssign;
            }
            _account = _user.Id + "=" + _username + " " + _password;

            ctl.WriteLine(_path._accountPath, _account);

            Console.WriteLine("Account created!");
        }
        public void AddUser()
        {
            User _user = new User();
            string _option;
            Console.WriteLine("Choose account's permission: \n1.ADMIN\n2.OTHERS");
            Console.Write("Choose your option: ");
            chooseOption:
            _option = Console.ReadLine();
            switch (_option)
            {
                case "1":
                    _user.Id += "AD";
                    AddAdmin(ref _user);
                    break;

                case "2":
                    ctl.sign_up(ref _user);
                    break;

                default:
                    Console.Write("Invalid option! Please choose again: ");
                    goto chooseOption;
            }
        }
        public void RemoveUser() 
        {
            ShowAccountFile();
            RemoveSpecificUser();
        }
        
        public void ShowTeacher()
        {
            FileStream fs = new FileStream(_path._teacherInfo, FileMode.Open, FileAccess.Read, FileShare.None);
            using (StreamReader sr = new StreamReader(fs))
            {
                string _userName = "", _userId = "", _line = "", _userPhoneNumber = "", _userAge = "";
                Console.WriteLine("||   {0, -10} || {1,-20} || {2,-4} || {3,-15} ||", "ID", "Name", "Age", "Phone Number");
                while ((_line = sr.ReadLine()) != null)
                {
                    int[] pos = new int[10];
                    findAll(_line, ref pos);
                    _userId = _line.Substring(0, pos[0]);
                    _userName = _line.Substring(pos[0] + 1, pos[1] - 1 - pos[0]);
                    _userAge = _line.Substring(pos[1] + 1, pos[2] - pos[1] - 1);
                    _userPhoneNumber = _line.Substring(pos[2] + 1, _line.Length - pos[2] - 1);
                    Console.WriteLine("||   {0, -10} || {1,-20} || {2,-4} || {3, -15} ||", _userId, _userName, _userAge, _userPhoneNumber);
                }
            }
        }

    }
}
