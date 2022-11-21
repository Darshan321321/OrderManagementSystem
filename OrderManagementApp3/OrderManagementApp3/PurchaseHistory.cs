using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrderManagementApp3
{
    internal class PurchaseHistory
    {
    
        private DateTime dt = new DateTime();

        public DateTime DT { get; set; }

        private int customer_id;

        public int Customer_id { get { return customer_id; } set { customer_id = value; } }

        private enum type
        {
            buy,
            sold
        }

        private type Type{ get; set; }

        private int item_id;

        public int Item_id { get { return item_id; } set { item_id = value; } }

        private int quantity;
        public int Quantity { get { return quantity; } set { quantity = value; } }

        public void UpdatefromCustomer(DateTime dts, Dictionary<string,int> name,string types, string usernames )
        {

         

            foreach (KeyValuePair<string, int> pair in name)
            {
                
                File.AppendAllText("itemHistory.csv", $"{dts},{usernames},{Convert.ToInt32(pair.Key)},{pair.Value},{types}\n");
              
             
            }

        }

        public void StockupdatefromCustomer( Dictionary<string, int> name)
        {



            foreach (KeyValuePair<string, int> pair in name)
            {

               
                UpdateStockfromCustomer(pair.Key, pair.Value);

            }

        }
        public void UpdateStockfromCustomer(string item_ids, int stockss)
        {

            String path = "itemFile.csv";
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

                            if (split[0] == item_ids)
                            {
                                split[3] = (Convert.ToInt32(split[3]) - stockss).ToString();
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


        }
        public void UpdatefromAdmin(DateTime dts,string adminname,string itemname,int quantity,string types)
        {
            File.AppendAllText("itemHistory.csv", $"{dts},{adminname},{itemname},{quantity},{types}\n");
        }

        public void ShowHistory()
        {
           
            var lines = File.ReadAllLines("itemHistory.csv");
            foreach (var line in lines)
            {
                var values = line.Split('\u002C');
                try
                {
                    Console.WriteLine($"{values[0]}  {values[1]} \t {values[2]} \t {values[3]} \t {values[4]}");
                }
                catch(IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }


    }
}
