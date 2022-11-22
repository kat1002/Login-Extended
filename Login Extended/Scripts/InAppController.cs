using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Extended
{
    public class InAppController
    {
        private string chooseOption()
        {
        Option:
            string _option = Console.ReadLine();
            _option.Trim();
            foreach (char item in _option)
            {
                if(item < '0' || item > '9')
                {
                    Console.Write("Invalid option! Please choose again: ");
                    goto Option;
                }
            }

            return _option;
        }

        private void AsAdmin(User _user)
        {
            string _option = "";
            //AdminOption
            Admin ad = new Admin();

            Console.WriteLine("OPTION:");
            Console.WriteLine("1.Add User\n2.Remove User\n3.Show all Teacher info\n4.Show all Student info\n5.Exit");
            Console.Write("Choose your option: ");
            
            chooseOption:
            _option = chooseOption();

            switch(_option)
            {
                case"1":
                    ad.AddUser();
                    break;

                case"2":
                    ad.RemoveUser();
                    break;

                case "3":
                    ad.ShowTeacher();
                    break;

                case "4":
                    ad.ShowStudent();
                    break;

                case "5":
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.Write("Invalid option! Please choose again: ");
                    goto chooseOption;
            }
        }

        private void AsTeacher(User _user)
        {
            //TeacherOption
            Teacher te = new Teacher();
            string _option = "";

            Console.WriteLine("OPTION:");
            Console.WriteLine("1.Show all Student Info\n2.Message\n3.Read message\n4.Clear all Messages\n5.Exit");
            Console.Write("Choose your option: ");
            
        chooseOption:
            _option = chooseOption();

            switch (_option)
            {
                case "1":
                    te.ShowStudent();
                    break;

                case "2":
                    te.Message(_user);
                    break;

                case "3":
                    te.ReadMessage();
                    break;

                case "4":
                    te.ClearAllMessage();
                    break;

                case "5":
                    System.Environment.Exit(0);
                    break;

                default:
                    Console.Write("Invalid option! Please choose again: ");
                    goto chooseOption;
            }
        }

        private void AsStudent(User _user)
        {
            //StudentOption
            Student hs = new Student();
            string _option = "";

            Console.WriteLine("OPTION:");
            Console.WriteLine("1.Read message\n2.Send message\n3.Exit");
            Console.Write("Choose your option: ");

        chooseOption:
            _option = chooseOption();
            
            switch (_option) 
            {
                case "1":
                    hs.ReadMessage();
                    break;

                case "2":
                    hs.Message(_user);
                    break;

                case "3":
                    System.Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid option! Please choose again: ");
                    goto chooseOption;
            }
        }

        public void Run(string _accessType, User _user)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                switch (_accessType)
                {
                    case "ADMIN":
                        AsAdmin(_user);
                        break;

                    case "TEACHER":
                        AsTeacher(_user);
                        break;

                    case "STUDENT":
                        AsStudent(_user);
                        break;
                }

                Console.ReadKey();
            }
        }
    }
}
