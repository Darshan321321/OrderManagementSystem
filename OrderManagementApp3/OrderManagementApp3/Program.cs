using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.AccessControl;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;


namespace OrderManagementApp3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Local More shop");

            CustomerRegister customerRegister = new CustomerRegister();
            AdminRegister adminRegister = new AdminRegister();
            Item item = new Item();
            ItemCategory itemCategory = new ItemCategory();
            PurchaseHistory purchase = new PurchaseHistory();

            Dictionary<string, int> item_price = new Dictionary<string, int>();

            try
            { 
            int a;
            Console.WriteLine("1.Customer Login \n2.Admin Login");
            a = Convert.ToInt32(Console.ReadLine());
                switch (a)
                {
                    case 1:

                        int countl = 0; bool logindecision, accountblock;
                        do
                        {
                            Console.WriteLine("Enter Username :");
                            customerRegister.Username = Console.ReadLine();
                            Console.WriteLine("Enter Password :");
                            customerRegister.Password = customerRegister.EncryptPassword(Console.ReadLine());
                            countl++;
                            (logindecision, accountblock) = customerRegister.CheckLogin();

                            if (accountblock == true)
                            {
                                Console.WriteLine("You're account is blocked"); break;
                            }

                            if (!logindecision && countl < 3)
                            { Console.WriteLine($"Wrong username or password\nYou still have {3 - countl} chances left"); }
                        } while (!logindecision && countl < 3);

                        bool continuebuying = true;
                        if (logindecision == true && accountblock == false)
                        {
                            item.ShowList();
                            do
                            {
                                Console.WriteLine("What do you want to buy,Give the number :");
                                string number = Console.ReadLine();

                                Console.WriteLine("Specify the quantity");
                                int quantity = Convert.ToInt32(Console.ReadLine());
                                int actualQuantity = item.getStockbyId(Convert.ToInt32(number));
                                if (actualQuantity <= quantity)
                                {
                                    Console.WriteLine($"Sorry the stock of the item is {actualQuantity}");
                                }

                                Console.WriteLine("Continue shopping now?\nPress Y if yes and press N if no");
                                char decision = Convert.ToChar(Console.Read());
                                item_price.Add(number, quantity);
                                Console.ReadLine();

                                if (decision is 'N')
                                {
                                    continuebuying = false;
                                }

                            } while (continuebuying);


                        }
                        else if (accountblock == true)
                        {
                            break;
                        }

                        double bill = 0;
                        foreach (KeyValuePair<string, int> kv in item_price)
                        {

                            bill += kv.Value * item.getpricebyId(Convert.ToInt32(kv.Key));

                        }
                        DateTime present = DateTime.Now;
                        string type = "Sold";
                        purchase.UpdatefromCustomer(present, item_price, type, customerRegister.Username);
                        purchase.StockupdatefromCustomer(item_price);
                        Console.WriteLine($"The bill would be ${bill}");
                        Console.WriteLine("Thanks for shopping,See you again");

                        break;


                    case 2:
                        int count = 0;
                        do
                        {
                            Console.WriteLine("Admin Username :");
                            adminRegister.AdminName = Console.ReadLine();
                            Console.WriteLine("Admin Password :");
                            adminRegister.AdminPassword = adminRegister.EncryptPassword(Console.ReadLine());
                            count++;
                            if (!adminRegister.CheckLogin() && count < 3)
                            { Console.WriteLine($"Wrong username or Password\nYou still have {3 - count} chances left"); }
                        } while (!adminRegister.CheckLogin() && count < 3);


                        if (adminRegister.CheckLogin())
                        {
                            bool logout = false;
                            do
                            {
                                int d;
                                Console.WriteLine("1.Register new Admin\n2.Register a Customer\n3.Add an item \n4.Update stock in item table\n5.Add to Category table\n6.View item purchase history\n7.Block a Customer Login\n8.Logout");
                                d = Convert.ToInt32(Console.ReadLine());

                                switch (d)
                                {
                                    case 1:
                                        adminRegister.AdminId = adminRegister.get_lines("AdminLoginCred.csv");

                                        do
                                        {
                                            Console.WriteLine("Enter AdminName: ");
                                            adminRegister.AdminName = Console.ReadLine();
                                        } while (!adminRegister.CheckUsernameAvailabiltyinAdminRegister());
                                        bool decision;
                                        do
                                        {
                                            Console.WriteLine("Enter you password:");
                                            string passWord = Console.ReadLine();
                                            decision = checkPasswords(passWord);
                                            if (decision == true)
                                            {
                                                adminRegister.AdminPassword = adminRegister.EncryptPassword(passWord);
                                            }
                                            //else can be used
                                            if (decision == false)
                                            {
                                                Console.WriteLine("Please enter the password which has atleast 1 Uppercase Letter, atleast 1 Symbol, atleast 1 number");
                                            }
                                        } while (decision == false);


                                        adminRegister.RegisterAdmin();

                                        break;
                                    case 2:
                                        customerRegister.Cust_id = customerRegister.get_lines("CustomerLoginCred.csv");
                                        do
                                        {
                                            Console.WriteLine("Enter your Username: ");
                                            customerRegister.Username = Console.ReadLine();
                                        } while (!customerRegister.CheckUsernameAvailabiltyinCustomerRegister());
                                        bool decision2;
                                        do {
                                            Console.WriteLine("Enter you password:");
                                            string passWords;
                                            passWords = Console.ReadLine();
                                            decision2 = checkPasswords(passWords);
                                            if (decision2 == true)
                                            {
                                                customerRegister.Password = customerRegister.EncryptPassword(Console.ReadLine());
                                            }
                                            //
                                            if (decision2 == false)
                                            {
                                                Console.WriteLine("Please enter the password which has atleast 1 Uppercase Letter, atleast 1 Symbol, atleast 1 number");
                                            }
                                        } while (decision2 == false);

                                        customerRegister.Status = true;

                                        customerRegister.RegisterUser();

                                        break;
                                    case 3:

                                        item.ItemId = item.get_lines("itemFile.csv");
                                        Console.WriteLine("Enter the Price of an Item :");
                                        item.Price = Convert.ToDouble(Console.ReadLine());
                                        Console.WriteLine("Enter the Name of an Item : ");
                                        item.Name = Console.ReadLine();
                                        Console.WriteLine("Enter the Quantity of an Item present :");
                                        item.Quantity = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Enter the Status of an item:");
                                        item.Status = Convert.ToBoolean(Console.ReadLine());
                                        Console.WriteLine("Enter Description of an item :");
                                        item.Description = Console.ReadLine();

                                        item.AddItem();

                                        break;

                                    case 4:
                                        Console.WriteLine("What is the name of the product , whose stock you want to change ?");
                                        item.Name = Console.ReadLine();

                                        Console.WriteLine("What is the stock quantity being added to the previous one");
                                        item.Quantity = int.Parse(Console.ReadLine());
                                        DateTime presentDT = DateTime.Now;
                                        item.UpdateItemQuantity();
                                        purchase.UpdatefromAdmin(presentDT, adminRegister.AdminName, item.Name, item.Quantity, "Buy");

                                        break;
                                    case 5:
                                        itemCategory.Category_Id = itemCategory.generateId("ItemCategoryFile.csv");
                                        Console.WriteLine("Enter the Category Name");
                                        itemCategory.Cateory_name = Console.ReadLine();
                                        itemCategory.Status = true;

                                        itemCategory.AddCategory();

                                        break;
                                    case 6:
                                        purchase.ShowHistory();

                                        break;
                                    case 7:
                                        Console.WriteLine("Enter the username of the person you want to block");
                                        customerRegister.Username = Console.ReadLine();
                                        customerRegister.BlockCustomer();
                                        break;
                                    case 8:

                                        logout = true;
                                        Console.WriteLine("Logout Successful!");
                                        break;
                                    default:
                                        Console.WriteLine("Hit the right number");
                                        break;

                                }

                            } while (!logout);
                        }
                        else
                        {
                            Console.WriteLine("You have lost all the chances, looser hahahhahahahaha");
                        }


                        break;
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Program.Main(args);
                //Console.WriteLine("Please give the correct input :");
                
                
            }
      
        }
        //public static bool passwordCheck(string passWord)
        //{
        //    int validConditions = 0;
        //    foreach (char c in passWord)
        //    {
        //        if (c >= 'a' && c <= 'z')
        //        {
        //            validConditions++;
        //            break;
        //        }
        //    }
        //    foreach (char c in passWord)
        //    {
        //        if (c >= 'A' && c <= 'Z')
        //        {
        //            validConditions++;
        //            break;
        //        }
        //    }
        //    if (validConditions == 0) return false;
        //    foreach (char c in passWord)
        //    {
        //        if (c >= '0' && c <= '9')
        //        {
        //            validConditions++;
        //            break;
        //        }
        //    }
        //    if (validConditions == 1) return false;
        //    if (validConditions == 2)
        //    {
        //        char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever    
        //        if (passWord.IndexOfAny(special) == -1) return false;
        //    }
        //    return true;
        //}

        public static bool checkPasswords(string pass)
        {
            //bool decision = true ;
            //    string MatchNumberPattern = "^.*(?=.{10,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$";
            //string MatchNumberPattern2 = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
            //if (!string.IsNullOrEmpty(psswd))
            //{
            //    if (!Regex.IsMatch(psswd, MatchNumberPattern2))
            //    {
            //        decision = false;
            //    }
            //    else
            //        decision = true;
            //}
            //return decision;
            //Regex validateGuidRegexes = new Regex("^[0-9a-zA-Z!@#$%^&*]");

            Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            if(validateGuidRegex.IsMatch(pass))
            {
                return true;
            }
            else
            {
                return false;
            }
            //if (Regex.IsMatch(pass, "^[a-z]*$") && Regex.IsMatch(pass, "^[0-9]*$") && Regex.IsMatch(pass, "^[!@#$%^&*]*$") && Regex.IsMatch(pass, "^[A-Z]*$"))
            //{
            //    return true;
            //}
            //else return false;
            //int count = 0;

            //if (8 <= pass.Length && pass.Length <= 32)
            //{
            //    if (Regex.Equals(pass, "^[a-z]*$"))
            //        count++;
            //    if (Regex.Equals(pass, "^[0-9]*$"))
            //        count++;
            //    if (Regex.Equals(pass, "^[!@#$%^&*]*$"))
            //        count++;
            //    if (Regex.Equals(pass, "^[A-Z]*$"))
            //          count++;
            //}

            //return count >= 3;
        }
    }
      

}

   
