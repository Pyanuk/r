using System;
using System.Collections.Generic;
using System.IO;

public class Order
{
    private List<int> cakeOptions;
    private decimal totalPrice;

    public Order()
    {
        cakeOptions = new List<int>();
        totalPrice = 0;
    }

    private void PrintMenu(Menu menu)
    {
        Console.Clear();
        menu.Print();
    }

    private int GetUserChoice()
    {
        ConsoleKeyInfo keyPressed = Console.ReadKey();
        if (keyPressed.Key == ConsoleKey.Escape)
        {
            return -1;
        }
        else if (keyPressed.Key == ConsoleKey.Enter)
        {
            return 0;
        }
        else if (int.TryParse(keyPressed.KeyChar.ToString(), out int choice))
        {
            return choice;
        }
        else
        {
            return -2;
        }
    }

    private void ChooseCakeOptions(Menu menu)
    {
        int choice;
        bool isSubMenu = false;

        do
        {
            if (!isSubMenu)
            {
                PrintMenu(menu);
            }

            choice = GetUserChoice();

            if (choice == 0)
            {
                isSubMenu = true;
                Console.Clear();
                Console.WriteLine("Выберите подпункт или нажмите Enter для возврата в основное меню:");
                menu.PrintSubMenu();
            }
            else if (menu.IsValidOption(choice))
            {
                cakeOptions.Add(choice);
                totalPrice += menu.GetOptionPrice(choice);
                isSubMenu = false;
            }

        } while (choice != -1);
    }

    public void MakeOrder(Menu menu)
    {
        ChooseCakeOptions(menu);
        Console.Clear();
        Console.WriteLine("Суммарная стоимость заказа: " + totalPrice.ToString("C"));
        SaveOrderToFile();
    }

    private void SaveOrderToFile()
    {
        string orderInfo = string.Join(", ", cakeOptions);
        string fileName = "История заказов.txt";

        using (StreamWriter writer = new StreamWriter(fileName, true))
        {
            writer.WriteLine(orderInfo);
        }

        Console.WriteLine("Заказ сохранен в файл " + fileName);
    }
}

public class Menu
{
    private List<MenuItem> menuItems;

    public Menu()
    {
        menuItems = new List<MenuItem>();
    }

    public void AddMenuItem(MenuItem menuItem)
    {
        menuItems.Add(menuItem);
    }

    public void Print()
    {
        Console.WriteLine("Выберите пункт меню:");
        for (int i = 0; i < menuItems.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {menuItems[i].Description}");
        }
    }

    public void PrintSubMenu()
    {
        int index = 1;
        foreach (MenuItem menuItem in menuItems)
        {
            List<SubMenuItem> subMenuItems = menuItem.SubMenuItems;
            if (subMenuItems.Count > 0)
            {
                Console.WriteLine($"{index}. {menuItem.Description}:");
                foreach (SubMenuItem subMenuItem in subMenuItems)
                {
                    Console.WriteLine($"   - {subMenuItem.Description}: {subMenuItem.Price.ToString("C")}");
                    index++;
                }
            }
        }
    }

    public bool IsValidOption(int choice)
    {
        return choice >= 1 && choice <= menuItems.Count;
    }

    public decimal GetOptionPrice(int choice)
    {
        return menuItems[choice - 1].Price;
    }
}

public class MenuItem
{
    internal decimal Price;

    public string Description { get; set; }
    public List<SubMenuItem> SubMenuItems { get; set; }

    public MenuItem(string description)
    {
        Description = description;
        SubMenuItems = new List<SubMenuItem>();
    }
}

public class SubMenuItem
{
    public string Description { get; set; }
    public decimal Price { get; set; }

    public SubMenuItem(string description, decimal price)
    {
        Description = description;
        Price = price;
    }
}

public class ConsoleMenu
{
    public static void Main(string[] args)
    {
        Menu menu = CreateMenu();
        bool continueOrdering = true;

        while (continueOrdering)
        {
            Order order = new Order();
            order.MakeOrder(menu);

            Console.WriteLine("Хотите оформить еще один заказ? (Да/Нет)");
            string choice = Console.ReadLine();

            continueOrdering = (choice.ToLower() == "да");
        }
    }

    private static Menu CreateMenu()
    {
        Menu menu = new Menu();

        MenuItem cakeShape = new MenuItem("Форма торта");
        cakeShape.SubMenuItems.Add(new SubMenuItem("Круглая", 500));
        cakeShape.SubMenuItems.Add(new SubMenuItem("Квадратная", 600));
        cakeShape.SubMenuItems.Add(new SubMenuItem("Сердце", 700));
        menu.AddMenuItem(cakeShape);

        MenuItem cakeSize = new MenuItem("Размер торта");
        cakeSize.SubMenuItems.Add(new SubMenuItem("Маленький", 1000));
        cakeSize.SubMenuItems.Add(new SubMenuItem("Средний", 1500));
        cakeSize.SubMenuItems.Add(new SubMenuItem("Большой", 2000));
        menu.AddMenuItem(cakeSize);

        MenuItem cakeFlavor = new MenuItem("Вкус торта");
        cakeFlavor.SubMenuItems.Add(new SubMenuItem("Шоколадный", 200));
        cakeFlavor.SubMenuItems.Add(new SubMenuItem("Ванильный", 150));
        cakeFlavor.SubMenuItems.Add(new SubMenuItem("Фруктовый", 250));
        menu.AddMenuItem(cakeFlavor);

        MenuItem cakeQuantity = new MenuItem("Количество тортов");
        cakeQuantity.SubMenuItems.Add(new SubMenuItem("1", 0));
        cakeQuantity.SubMenuItems.Add(new SubMenuItem("2", 0));
        cakeQuantity.SubMenuItems.Add(new SubMenuItem("3", 0));
        menu.AddMenuItem(cakeQuantity);

        MenuItem cakeGlaze = new MenuItem("Глазурь торта");
        cakeGlaze.SubMenuItems.Add(new SubMenuItem("Шоколадная", 50));
        cakeGlaze.SubMenuItems.Add(new SubMenuItem("Ванильная", 30));
        cakeGlaze.SubMenuItems.Add(new SubMenuItem("Карамельная", 40));
        menu.AddMenuItem(cakeGlaze);

        MenuItem cakeDecor = new MenuItem("Декор торта");
        cakeDecor.SubMenuItems.Add(new SubMenuItem("Цветочный", 100));
        cakeDecor.SubMenuItems.Add(new SubMenuItem("Фигурный", 80));
        cakeDecor.SubMenuItems.Add(new SubMenuItem("Тематический", 120));
        menu.AddMenuItem(cakeDecor);

        return menu;
    }
}