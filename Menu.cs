using System;
using System.Collections.Generic;
using System.Text;

namespace Dictionary
{
    public class Menu
    {
        private string caption { get; set; }
        private List<MenuItem> menuItems = new List<MenuItem>();

        public Menu(string caption)
        {
            this.caption = caption;
        }
        public void Show()
        {
            Console.WriteLine(caption);
            for (int i = 0; i < menuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i]}");
            }
        }

        public MenuItem Selection()
        {
            string input = Console.ReadLine();
            int index;
            if (!int.TryParse(input, out index))
            {
                Console.Error.WriteLine($"Invalid input '{input}'");
                return null;
            }
            else {
                index = Convert.ToInt32(input) - 1;
                if (index < 0 || index >= menuItems.Count)
                {
                    Console.Error.WriteLine($"Index {input} is not valid input");
                    return null;
                }
            }
            return menuItems[index];
        }

        public MenuItem Execute()
        {
            MenuItem item = null;
            do
            {
                Show();
                item = Selection();
            } while (item == null);

            return item;
        }

        public void Add(MenuItem menuItem)
        {
            this.menuItems.Add(menuItem);
        }
    }
}
