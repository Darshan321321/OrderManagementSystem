using System;
using System.Collections.Generic;
using System.Globalization;

using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp3
{
    internal class CustomerRegister
    {
        private int cust_id;
        private string username;
        private string password;
        private bool status;

        public int Cust_id { get { return Cust_id; } set { cust_id = value; } }

        public string Username { get { return username; } set { username = value; } }

        public string Password { get { return password; } set { password = value; } }

        public bool Status { get { return status; } set { status = value; } }

        public bool CheckUsernameAvailabiltyinCustomerRegister()
        {
            var lines = File.ReadAllLines("CustomerLoginCred.csv");

            foreach (var line in lines)
            {

                var values = line.Split('\u002C');
                try
                {
                    if (values[1] == username)
                    {
                        Console.WriteLine("This username is already taken");
                        return false;
                    }

                    else
                        continue;
                }
                catch(IndexOutOfRangeException ie)
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

        public void RegisterUser()
        {

            File.AppendAllText("CustomerLoginCred.csv", $"{cust_id},{username},{password},{status}\n");
            Console.WriteLine("Registered Succesfully");
        }
        public void outputFile()
        {
            Console.WriteLine("CustomerLoginCred.csv :");
            var lines = File.ReadAllLines("CustomerLoginCred.csv");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
        public void BlockCustomer()
        {

            String path = "CustomerLoginCred.csv";
            List<String> lines = new List<String>();

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(","))
                        {
                            String[] split = line.Split(',');

                            if (split[1] == username)
                            {
                                split[3] = false.ToString();
                                line = String.Join(",", split);
                            }
                        }

                        lines.Add(line);
                    }
                }
                using (StreamWriter writer = new StreamWriter(path, false))

                {
                    foreach (String line in lines)
                        writer.WriteLine(line);
                }

            }
            Console.WriteLine("Blocked the customer Successfully");

        }
        public (bool,bool) CheckLogin()
        {
            var lines = File.ReadAllLines("CustomerLoginCred.csv");
            bool found = false, accountdisabled = false ;
            foreach (var line in lines)
            {

                var values = line.Split('\u002C');
               
                
                if (values[1] == username && values[2] == password)
                {
                    found = true;
                    if (values[3] == true.ToString())
                    {
                        accountdisabled = false;
                    }
                    else if (values[3] == false.ToString())
                    {
                        accountdisabled = true;
                        break;
                    }
                }
            }
          
            return (found,accountdisabled);

        }
    }
}
