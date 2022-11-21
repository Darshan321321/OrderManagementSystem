using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApp3
{
    internal class ItemCategory
    {
        private int Category_id;
        private string Category_name;
        private bool status;

        public int Category_Id { get { return Category_id; } set { Category_id = value; } }
        public string Cateory_name { get { return Category_name; } set { Category_name = value; } }
        public bool Status { get { return status; } set { status = value; } }

        public int generateId(string file)
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
        public void AddCategory()
        {

            File.AppendAllText("ItemCategoryFile.csv", $"{Category_id},{Category_name},{status}\n");
            Console.WriteLine("Added the category Succesfully");



        }
        


    }
}
