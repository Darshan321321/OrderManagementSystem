using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OrderManagementApp3
{
    internal class Item
    {
        private int item_id;
        private double price;
        private string name;
        private string description;
        private int quantity;
        private bool status;
        

        public int ItemId { get { return item_id; } set { item_id = value; } }
        public double Price { get { return price; } set { price = value; } }
        public string Name { get { return name; } set { name = value; } }
        public int Quantity { get { return quantity; } set { quantity = value; } }
        public bool Status { get { return status; } set { status = value; } }
        public string Description { get { return description; } set { description = value; } }
        //
        public void AddItem()
        {
            File.AppendAllText("itemFile.csv", $"{ItemId},{price},{name},{quantity},{status},{description}\n");
            Console.WriteLine("Item Added Succesfully");
        }

        public void UpdateItemQuantity()
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

                            if (split[2]== name)
                            {
                                split[3] = (Convert.ToInt32(split[3])+ quantity).ToString();
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
            Console.WriteLine("item's Stock updated");

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

        public void ShowList()
        {
            Console.WriteLine("Item\t-Price");
            var lines = File.ReadAllLines("itemFile.csv");
            foreach (var line in lines)
            {
                var values = line.Split('\u002C');
                try
                {
                    if (values[4].Contains("True") && !(Convert.ToInt32(values[3])<=0))
                    {
                        Console.WriteLine($"{values[0]}.{values[2]}\t-${values[1]}");

                    }
                }
                catch (IndexOutOfRangeException )
                {
                    break;
                }
            }
        }

        public double getpricebyId(int id)
        {
            double price = 0;
           
            var lines = File.ReadAllLines("itemFile.csv");
            foreach (var line in lines)
            {
                var values = line.Split('\u002C');

                if (values[0] == id.ToString())
                    price =Convert.ToDouble( values[1]);

            }
            return price;
        }

        public int getStockbyId(int id)
        {
            int quantity= 0;
           
            var lines = File.ReadAllLines("itemFile.csv");
            foreach (var line in lines)
            {
                var values = line.Split('\u002C');

                if (values[0] == id.ToString())
                    quantity = Convert.ToInt32(values[3]);

            }
            return quantity;
        }


        public void UpdateStockfromCustomer(string item_ids,int stockss)
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
                                split[3] = (Convert.ToInt32(split[3]) - quantity).ToString();
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


    }
}
