using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.IO;
namespace student_grade_tracker
{
    class Program
    {
        static List<string> studentsNames = new List<string>();
        static List<int> studentsGrades = new List<int>();
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
                =======================================
                === Система обліку оцінок студентів ===
                =======================================
                = 0 - Вихід з програми                =
                = 1 - Додати студента та оцінку       =
                = 2 - Видалити студента за ID         =
                = 3 - Переглянути список студентів    =
                = 4 - Очистити список студентів       =
                = 5 - Розрахувати статистику та аналіз=
                = 6 - Експорт списку оцінок студентів =
                =======================================
                """);
                Console.Write("Введіть номер операції(0-6): ");
                string inputMenu = Console.ReadLine();
                Console.WriteLine(new string('=', 39));
                if (string.IsNullOrWhiteSpace(inputMenu) || !int.TryParse(inputMenu, out int intMode) || intMode < 0 || intMode > 6)
                {
                    ShowError($"Невірний ввід! Введіть номер операції(0-6).");
                    continue;
                }
                switch (intMode)
                {
                    case 0:
                        ExitProgram();
                        break;
                    case 1:
                        AddStudentAndGrade();
                        break;
                    case 2:
                        DelStudentAndGrade();
                        break;
                    case 3:
                        CheckListStudents();
                        break;
                    case 4:
                        ClearListStudents();
                        break;
                    case 5:
                        StatisticsAndAnalysis();
                        break;
                    case 6:
                        ExportGrades();
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
        static void AddStudentAndGrade()
        {
            do
            {
                Console.Clear();
                int nextStudentID = studentsNames.Count + 1;
                string studentName = ReadStudentName(nextStudentID);
                Console.WriteLine(new string('=', 39));
                int studentGrade = ReadStudentGrade(studentName, nextStudentID);
                studentsNames.Add(studentName);
                studentsGrades.Add(studentGrade);
                Console.WriteLine($"Студент ID {studentsNames.Count} {studentName} з оцінкою {studentGrade} доданий.");
                Thread.Sleep(2000);
                Console.Clear();
                while (true)
                {
                    Console.WriteLine(new string('=', 39));
                    Console.Write("Бажаєте додати студента та оцінку ще раз? (y/n): ");
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    string input = keyInfo.KeyChar.ToString();
                    if (input.ToLower() == "y") break;
                    else if (input.ToLower() == "n")
                    {
                        Console.WriteLine("\nОперація скасована.");
                        Thread.Sleep(1500);
                        return;
                    }
                    else
                    {
                        ShowError("Невірний ввід!");
                        continue;
                    }
                }
            }
            while (true);
        }
        static string ReadStudentName(int studentID)
        {
            while (true)
            {
                Console.Write($"Введіть ім'я студента з ID {studentID}: ");
                string inputName =Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(inputName))
                {
                    return inputName.Trim();
                }
                ShowError("Невірний ввід!");
            }
        }
        static int ReadStudentGrade(string studentName, int studentID)
        {
            while (true)
            {
                Console.Write($"Введіть оцінку студента {studentName} ID {studentID}(0-100): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int grade) && grade >= 0 && grade <= 100)
                {
                    return grade;
                }
                ShowError("Некоректна оцінка! Введіть число від 0 до 100.");
            }
        }
        static void DelStudentAndGrade()
        {
            do
            {
                Console.Clear();
                if (IsListStudentsEmpty()) return;
                Console.WriteLine("Актуальний список студентів та оцінок:");
                PrintStudentsList();
                Console.WriteLine(new string('=', 39));
                Console.Write("Введіть ID студента для видалення(або Q для припинення): ");
                string input = Console.ReadLine();
                if (input.ToLower() == "q") break;
                if (int.TryParse(input, out int studentID) && studentID > 0 && studentID <= studentsNames.Count)
                {
                    int index = studentID - 1;
                    string name = studentsNames[index];
                    int grade = studentsGrades[index];
                    studentsNames.RemoveAt(index);
                    studentsGrades.RemoveAt(index);
                    Console.WriteLine($"Студент ID {studentID} {name} видалений разом з оцінкою {grade}.");
                }
                else
                {
                    ShowError("Невірний ввід!");
                }
            }
            while (true);
            return;
        }
        static void CheckListStudents()
        {
            Console.Clear();
            if (IsListStudentsEmpty()) return;
            Console.WriteLine("Cписок студентів та оцінок:");
            PrintStudentsList();
            Console.WriteLine($"""
            {new string('=', 39)}
            Натисніть будь-яку клавішу для повернення до меню...
            Натисніть Esc для виходу з програми...
            """);
            if (Console.ReadKey().Key == ConsoleKey.Escape) ExitProgram();
            return;
        }
        static void PrintStudentsList()
        {
            for (int i = 0; i < studentsNames.Count; i++)
            {
                Console.WriteLine($"ID {i + 1}.\t {studentsNames[i]} \t Оцінка: {studentsGrades[i]}");
            }
        }
        static void ClearListStudents()
        {
            Console.Clear();
            if (IsListStudentsEmpty()) return;
            do
            {
                Console.Clear();
                Console.WriteLine(new string('=', 39));
                Console.Write("Ви впевнені, що хочете очистити список студентів? (y/n): ");
                ConsoleKeyInfo clearListStudentsKeyInfo = Console.ReadKey();
                string inputClearListStudents = clearListStudentsKeyInfo.KeyChar.ToString();
                if (inputClearListStudents.ToLower() == "n")
                {
                    Console.WriteLine("\nОперація скасована.");
                    Thread.Sleep(1500);
                    return;
                }
                else if (inputClearListStudents.ToLower() == "y")
                {
                    studentsNames.Clear();
                    studentsGrades.Clear();
                    Console.WriteLine("Список студентів очищено.");
                    Thread.Sleep(2000);
                    break;
                }
                else
                {
                    ShowError("Невірний ввід!");
                    continue;
                }
            }
            while (true);
        }
        static void StatisticsAndAnalysis()
        {
            Console.Clear();
            if (IsListStudentsEmpty()) return;
            do
            {
                Console.Clear();
                Console.WriteLine("""
                ==========================================
                === Розрахування статистики та аналізу ===
                ==========================================
                = 0 - Вихід з програми                   =
                = 1 - Повернення до меню                 =
                = 2 - Найкращий студент за оцінкою       =
                = 3 - Середній бал групи                 =
                = 4 - Студенти з балом нижчим за середній=
                ==========================================
                """);
                Console.Write("Введіть номер операції(0-4): ");
                string inputMenu = Console.ReadLine();
                Console.WriteLine(new string('=', 39));
                if (string.IsNullOrWhiteSpace(inputMenu) || !int.TryParse(inputMenu, out int intMode) || intMode < 0 || intMode > 4)
                {
                    ShowError($"Невірний ввід! Введіть номер операції(0-4).");
                    continue;
                }
                switch (intMode)
                {
                    case 0:
                        ExitProgram();
                        break;
                    case 1:
                        return;
                    case 2:
                        ShowBestStudent();
                        break;
                    case 3:
                        ShowAverageGrade();
                        break;
                    case 4:
                        ShowStudentsBelowAverage();
                        break;
                }
            }
            while (true);
        }
        static void ShowBestStudent()
        {
            Console.Clear();
            if (IsListStudentsEmpty()) return;
            int bestStudentGrade = studentsGrades.Max();
            for(int i = 0; i < studentsGrades.Count; i++)
            {
                if (bestStudentGrade == studentsGrades[i])
                {
                    Console.WriteLine($"Найкращий студент за оцінкою: {studentsNames[i]} ID {i + 1} з балом {bestStudentGrade}.");
                }
            }
            Console.WriteLine(new string('=', 42) + "\nНатисніть будь-яку клавішу для повернення до меню...\nНатисніть Esc для виходу з програми...");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) ExitProgram();
            return;
        }
        static void ShowAverageGrade()
        {
            Console.Clear();
            if (IsListStudentsEmpty()) return;
            Console.WriteLine($"Середній бал групи: {studentsGrades.Average()}");
            Console.WriteLine(new string('=', 42) + "\nНатисніть будь-яку клавішу для повернення до меню...\nНатисніть Esc для виходу з програми...");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) ExitProgram();
            return;
        }
        static void ShowStudentsBelowAverage()
        {
            Console.Clear();
            if (IsListStudentsEmpty()) return;
            double avgStudentsGrade = studentsGrades.Average();
            bool isStudentBelowAverage = false;
            for (int i = 0; i < studentsGrades.Count; i++)
            {
                if (studentsGrades[i] < avgStudentsGrade)
                {
                    isStudentBelowAverage = true;
                    Console.WriteLine($"Студент ID {i + 1} {studentsNames[i]} з балом {studentsGrades[i]} нижчим за середній бал групи {avgStudentsGrade}.");
                }
            }
            if (!isStudentBelowAverage) Console.WriteLine("Немає студентів з балом нижчим за середній бал групи.");
            Console.WriteLine(new string('=', 42) + "\nНатисніть будь-яку клавішу для повернення до меню...\nНатисніть Esc для виходу з програми...");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) ExitProgram();
            return;
        }
        static bool IsListStudentsEmpty()
        {
            if (studentsNames.Count == 0)
            {
                ShowError("Список порожній!");
                return true;
            }
            return false;
        }
        static void ExportGrades()
        {
            Console.Clear();
            if (IsListStudentsEmpty()) return;
            string fileName = "grades.txt";
            string[] formatedStudentsGrades = new string[studentsGrades.Count];
            for (int i = 0; i < studentsGrades.Count; i++)
            {
                formatedStudentsGrades[i] = $"ID {i + 1} {studentsNames[i]} \t Оцінка: {studentsGrades[i]}";
            }
            File.WriteAllLines(fileName, formatedStudentsGrades);
            Console.WriteLine($"Список оцінок студентів успішно збережений в файл \"{fileName}\".");
            Console.WriteLine(new string('=', 39) + "\nНатисніть будь-яку клавішу для повернення до меню...");
            Console.ReadKey(true);
            return;
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