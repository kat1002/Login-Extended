using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Login_Extended
{
    internal class Program
    {
        static void Main(string[] args)
        {
        Start:
            Path _path = new Path();
            Main_Controller ctl = new Main_Controller();
            InAppController iac = new InAppController();
            User _user = new User();
            string _accessType = "";

            //Title
            string _title = "Sign-in Application";
            Console.SetCursorPosition((Console.WindowWidth - _title.Length) / 2, Console.CursorTop);
            Console.WriteLine(_title);

            //Input
            Console.WriteLine("\n1.Sign-up\n2.Sign-in\n3.Exit");
            Console.Write("Choose your option: ");
            string _option = "";
            ctl.chooseOption(ref _option);

            //Body
            switch (_option)
            {
                case "1":
                    ctl.sign_up(ref _user);
                    break;

                case "2":
                    if (ctl.sign_in(_path._accountPath, ref _accessType, ref _user))
                    {
                        Console.WriteLine("Press any button to continue!");
                        Console.ReadKey();
                        iac.Run(_accessType, _user);
                    }
                    break;
                case "3":
                    System.Environment.Exit(0);
                    break;
            }

        //EndProgram
        End:
            string _endOption;
            Console.Write("Do you want to end the program? (Y/N) ");
            _endOption = Console.ReadLine();
            _endOption.Trim();
            _endOption = _endOption.ToLower();
            if (_endOption == "y" || _endOption == "yes") System.Environment.Exit(0);
            if (_endOption == "n" || _endOption == "no")
            {
                Console.Clear();
                goto Start;
            }
            else
            {
                Console.WriteLine("Invalid Option!");
                goto End;
            }
        }
    }
}
