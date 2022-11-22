using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Extended
{
    public class User
    {
        private string _name;
        private int _age;
        private string _id;
        private string _phoneNumber;
        public Path _path = new Path();

        public string Name { get { return _name; } set { _name = value; } }
        public string PhoneNumber { get { return _phoneNumber;} set { _phoneNumber = value; } }
        public int Age { get { return _age;} set {_age = value;} }
        public string Id { get { return _id; } set { _id = value; } }
        
        
    }
}
