using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp3
{
    internal class AdminRegister
    {
        private int admin_id;
        private string admin_name;
        private string admin_password;

        public int AdminId { get { return admin_id; } set { admin_id = value; }}
        public string AdminName { get { return admin_name; } set { admin_name = value; } }
        public string AdminPassword { get { return admin_password; } set { admin_password = value; } }

        public bool CheckUsernameAvailabiltyinAdminRegister()
        {

            var lines = File.ReadAllLines("AdminLoginCred.csv");

            foreach (var line in lines)
            {

                var values = line.Split('\u002C');
                try
                {
                    if (values[1] == admin_name)
                    {
                        Console.WriteLine("This username is already taken");
                        return false;
                    }

                    else
                        continue;
                }
                catch (IndexOutOfRangeException )
                {
                    continue;
                }
            }
            return true;
        }
            public int get_lines(string file)
        {
            var lineCount = 0;
            using (var stream = new StreamReader(file))
            {
                while (stream.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            return lineCount;
        }
        public string EncryptPassword(string strEncrypt)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypt);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        public string DecryptPassword(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = Convert.ToBase64String(b);

            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public void RegisterAdmin()
        {

            File.AppendAllText("AdminLoginCred.csv", $"{admin_id},{admin_name},{admin_password}\n");
            Console.WriteLine("Registered Succesfully");
        }
        public void outputFile()
        {
            Console.WriteLine("AdminLoginCred.csv :");
            var lines = File.ReadAllLines("AdminLoginCred.csv");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        public bool CheckLogin()
        {
            var lines = File.ReadAllLines("AdminLoginCred.csv");
            bool found = false;
            foreach (var line in lines)
            {
                
                var values = line.Split('\u002C');
                if (values[1] == admin_name && values[2] == admin_password)
                {
                    found = true;
                    break;
                }
               
            }
            return found;
           
        }
    }
}
