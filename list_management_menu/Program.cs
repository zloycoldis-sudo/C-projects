using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
namespace list_management_menu
{
    class Program
    {
        static List<string> ToDoList = new ();
        static void Main(string[] args)
        {
            ShowMenu();
        }
        static void ShowMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("""
                ================================
                === Меню взаємодії з списком ===
                ================================
                =  0 - Вихід з програми        =
                =  1 - Додати справу до списку =
                =  2 - Видалити справу з списку=
                =  3 - Переглянути список      =
                =  4 - Очистити список         =
                =  5 - Сортувати список        =
                =  6 - Експортувати список     =
                =  7 - Імпортувати список      =
                ================================
                """);
                Console.Write("Введіть номер операції(0-7): ");
                string inputMenu = Console.ReadLine();
                Console.WriteLine(new string('=', 32));
                if (string.IsNullOrWhiteSpace(inputMenu) || !int.TryParse(inputMenu, out int intMenu) || intMenu < 0 || intMenu > 7)
                {
                    ShowError($"Невірний ввід! Введіть номер операції(0-7).");
                    continue;
                }
                switch (intMenu)
                {
                    case 0:
                        ExitProgram();
                        break;
                    case 1:
                        AddToList();
                        break;
                    case 2:
                        DelInList();
                        break;
                    case 3:
                        CheckList();
                        break;
                    case 4:
                        ClearList();
                        break;
                    case 5:
                        SortList();
                        break;
                    case 6:
                        ExportToFile();
                        break;
                    case 7:
                        ImportFromFile();
                        break;
                }
            }
            while (true);
        }
        static void ExitProgram()
        {
            Console.Clear();
            Environment.Exit(0);
        }
        static void AddToList()
        {
            do
            {
                Console.Clear();
                Console.Write($"Запишіть справу №{ToDoList.Count + 1} для додавання(або Q для припинення): ");
                string inputAddToList = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputAddToList))
                {
                    ShowError($"Невірний ввід! Введіть справу для додавання.");
                    continue;
                }
                if (inputAddToList.ToLower() == "q") break;
                ToDoList.Add(inputAddToList);
                Console.WriteLine($"Справа №{ToDoList.Count} \"{inputAddToList}\" додана.");
                Thread.Sleep(2000);
                Console.Clear();
            }
            while (true);
            Console.Clear();
            return;
        }
        static void DelInList()
        {
            do
            {
                Console.Clear();
                PrintToDoList();
                Console.WriteLine(new string('=', 32));
                Console.Write($"Введіть справу або її номер для видалення(або Q для припинення): ");
                string inputDelInList = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputDelInList))
                {
                    ShowError($"Невірний ввід! Введіть справу або її номер для видалення.");
                    continue;
                }
                if (inputDelInList.ToLower() == "q") break;
                if (ToDoList.Count == 0)
                {
                    ShowError($"Список порожній!");
                    return;
                }
                if (ToDoList.Contains(inputDelInList))
                {
                    int indexDelInList = ToDoList.IndexOf(inputDelInList);
                    ToDoList.RemoveAt(indexDelInList);
                    Console.WriteLine($"Справа №{indexDelInList + 1} \"{inputDelInList}\" видалена.");
                    Thread.Sleep(2000);
                }
                else if (int.TryParse(inputDelInList, out int intDelInList) && intDelInList > 0 && intDelInList <= ToDoList.Count)
                {
                    string DelInList = ToDoList[intDelInList - 1];
                    ToDoList.RemoveAt(intDelInList - 1);
                    Console.WriteLine($"Справа №{intDelInList} \"{DelInList}\" видалена.");
                    Thread.Sleep(2000);
                }
                else
                {
                    ShowError($"Невірний ввід! Введіть справу або її номер для видалення.");
                }
            }
            while (true);
            Console.Clear();
            return;
        }
        static void CheckList()
        {
            Console.Clear();
            PrintToDoList();
            Console.WriteLine(new string('=', 32) + "\nНатисніть будь-яку клавішу для повернення до меню...\nНатисніть Esc для виходу з програми.");
            if (Console.ReadKey().Key == ConsoleKey.Escape) ExitProgram();
            return;
        }
        static void ClearList()
        {
            if (ToDoList.Count == 0)
            {
                Console.WriteLine("Список порожній.");
                Thread.Sleep(2000);
                return;
            }
            do
            {
                Console.Write("Ви впевнені, що хочете очистити список? (y/n): ");
                string inputClearList = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputClearList) ||  inputClearList.ToLower() != "y" && inputClearList.ToLower() != "n" )
                {
                    ShowError($"Невірний ввід! Введіть y або n.");
                    continue;
                }
                if (inputClearList.ToLower() == "y")
                {
                    ToDoList.Clear();
                    Console.WriteLine("Список справ був очищений.");
                    Thread.Sleep(2000);
                }
                break;
            }
            while (true);
            Console.Clear();
            return;
        }
        static void SortList()
        {
            if (ToDoList.Count == 0)
            {
                Console.WriteLine("Список порожній!");
                Thread.Sleep(2000);
                return;
            }
            bool exitSortingMode = false;
            do
            {
                Console.Clear();
                PrintToDoList();
                Console.WriteLine(new string('=', 32));
                Console.WriteLine("Щоб відсортувати список від А до Я, натисніть 1...\nЩоб відсортувати список від Я до А, натисніть 2...\nЩоб повернутись до меню, натисніть 3...\nНатисніть Esc для виходу з програми.");
                Console.WriteLine(new string('=', 32));
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape) ExitProgram();
                string inputSortList = keyInfo.KeyChar.ToString();
                if (string.IsNullOrWhiteSpace(inputSortList) || !int.TryParse(inputSortList,out int intSortList) || intSortList < 1 || intSortList > 3)
                {
                    ShowError($"Невірний ввід! Введіть номер операції(1-3) або натисніть Esc.");
                    continue;
                }
                switch (intSortList)
                {
                    case 1:
                        ToDoList.Sort();
                        Console.WriteLine("\nСписок справ відсортовано від А до Я.");
                        break;
                    case 2:
                        ToDoList.Sort();
                        ToDoList.Reverse();
                        Console.WriteLine("\nСписок справ відсортовано від Я до А.");
                        break;
                    case 3:
                        exitSortingMode = true;
                        break;
                }
                Thread.Sleep(2000);
            }
            while (!exitSortingMode);
            Console.Clear();
            return;
        }
        static void ExportToFile()
        {
            Console.Clear();
            Console.WriteLine(new string('=', 32));
            if (ToDoList.Count == 0)
            {
                Console.WriteLine("Список порожній!");
                Thread.Sleep(2000);
                return;
            }
            string fileName = "todo_list.txt";
            File.WriteAllLines(fileName, ToDoList);
            Console.WriteLine($"Список справ успішно збережений в файл \"{fileName}\".");
            Thread.Sleep(2000);
            Console.Clear();
            return;
        }
        static void ImportFromFile()
        {
            Console.Clear();
            Console.WriteLine(new string('=', 32));
            string fileName = "todo_list.txt";
            if (!File.Exists(fileName))
            {
                ShowError($"Файл \"{fileName}\" не знайдений!");
                Thread.Sleep(2000);
                return;
            }
            if (ToDoList.Count > 0)
            {
                do
                {
                Console.Write("Якщо імпортувати список, то він буде перезаписаний! Поточний список буде створений з початку.\nБажаєте продовжити імпорт списку? (y/n): ");
                string inputImportList = Console.ReadLine();
                Console.WriteLine(new string('=', 32));
                if (string.IsNullOrWhiteSpace(inputImportList) ||  inputImportList.ToLower() != "y" && inputImportList.ToLower() != "n" )
                {
                    ShowError($"Невірний ввід! Введіть y або n.");
                    continue;
                }
                if (inputImportList.ToLower() == "n")
                {
                    Console.WriteLine("Операція скасована.");
                    Thread.Sleep(1500);
                    return;
                }
                break;
                }
                while (true);
            }
            string[] loadedToDoList = File.ReadAllLines(fileName);
            ToDoList = new List<string>(loadedToDoList);
            Console.WriteLine($"Список справ успішно імпортований з файлу \"{fileName}\".");
            Thread.Sleep(2000);
            Console.Clear();
            return;
        }
        static void PrintToDoList()
        {
            Console.WriteLine("Cписок справ: ");
            for (int i = 0; i < ToDoList.Count; i++)
            {
                Console.WriteLine($"{i + 1}.\t{ToDoList[i]}");
            }
        }
        static void ShowError(string messageError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(messageError);
            Console.ResetColor();
            Thread.Sleep(2000);
            Console.Clear();
        }
    }
}