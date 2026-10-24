using System;
using System.Threading;
namespace TicTacToe0._1
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowMenu();
        }
        static void ShowMenu()
        {
            do
            {
                Console.WriteLine("""
                ==Гра "Хрестики-Нолики"==
                =========================
                =         Меню          =
                =                       =
                =    1 - Почати гру     =
                =    2 - Вийти з гри    =
                =========================
                """);
                Console.Write("Виберіть опцію: ");
                if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 2)
                {
                    Console.WriteLine(new string('=',25 ));
                    ShowError("Невірний ввід! Виберіть опцію з меню.");
                    Console.Clear();
                    Console.WriteLine(new string('=',25 ));
                    continue;
                }
                Console.WriteLine(new string('=',25 ));
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Оберіть режим гри...");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GameSetup();
                        break;
                    case 2:
                        Exit();
                        break;
                }
            }
            while (true);
        }
        static void GameSetup()
        {
            string player1 = "";
            string player2 = "";
            do
            {
                Console.WriteLine("""
                ========Режим гри========
                =========================
                =    1 - Гра з ботом    =
                =    2 - Гра з гравцем  =
                =    3 - Назад до меню  =
                =========================
                """);
                Console.Write("Виберіть режим: ");
                if (!int.TryParse(Console.ReadLine(), out int mode) || mode < 1 || mode > 3)
                {
                    Console.WriteLine(new string('=',25 ));
                    ShowError("Невірний ввід! Виберіть режим з меню.");
                    Console.Clear();
                    Console.WriteLine(new string('=',25 ));
                    continue;
                }
                if (mode == 3)
                {
                    Console.Clear();
                    return;
                }
                Console.WriteLine(new string('=',25 ));
                switch (mode)
                {
                    case 1:
                    {
                        Console.WriteLine("Введіть свій Nickname до 10 символів: "); player1 = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(player1) || player1.Length > 10)
                        {
                            Console.WriteLine(new string('=',25 ));
                            ShowError("Невірний ввід! Введіть свій Nickname до 10 символів.");
                            Console.Clear();
                            Console.WriteLine(new string('=',25 ));
                            continue;
                        }
                        player2 = "BotV0.1";
                        break;
                    }
                    case 2:
                    {
                        Console.Write("Введіть Nickname першого гравця до 10 символів: "); player1 = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(player1) || player1.Length > 10)
                        {
                            Console.WriteLine(new string('=',25 ));
                            ShowError("Невірний ввід! Введіть свій Nickname до 10 символів.");
                            Console.Clear();
                            Console.WriteLine(new string('=',25 ));
                            continue;
                        }
                        Console.Write("Введіть Nickname другого гравця до 10 символів: "); player2 = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(player2) || player2.Length > 10)
                        {
                            Console.WriteLine(new string('=',25 ));
                            ShowError("Невірний ввід! Введіть свій Nickname до 10 символів.");
                            Console.Clear();
                            Console.WriteLine(new string('=',25 ));
                            continue;
                        }
                        break;
                    }
                }
                Console.Clear();
                Console.WriteLine("Запуск гри...");
                Thread.Sleep(1500);
                Console.Clear();
                char[,] board = new char[3, 3];
                StartGame(mode,player1, player2, board);
                break;
            }
            while (true);
        
        }
        static void StartGame(int mode, string player1, string player2, char[,] board )
        {
            char p1Choice = choiceTicTakToe(player1);
            char p2Choice = (p1Choice == 'X') ? 'O' : 'X';
            char currentNum = '1';
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i,j] = currentNum++;
                }
            }
            string winner = "";
            while (true)
            {
                DrawBoard(board);
                if (!PlayerTurn(player1, board, p1Choice)) return;
                if (CheckWin(board))
                {
                    winner = player1;
                    break;
                }
                if (CheckDraw(board)) break;
                DrawBoard(board);
                if (mode == 1)
                {
                    PlayerBOT(player2, board, p2Choice);
                }
                else if (mode == 2)
                {
                    if (!PlayerTurn(player2, board, p2Choice)) return;
                }
                if (CheckWin(board))
                {
                    winner = player2;
                    break;
                }
                if (CheckDraw(board)) break;
            }
            Console.Clear();
            DrawBoard(board);
            if (!string.IsNullOrEmpty(winner))
            {
                ShowWinner(winner);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Нічия!");
                Console.ResetColor();
                Thread.Sleep(2000);
                Console.Clear();
            }
            Console.WriteLine("Повернення в головне меню...");
            Thread.Sleep(2000);
            Console.Clear();
            return;
        }
        static char choiceTicTakToe(string player1)
        {
            do
            {
                Console.Write($"{player1}, виберіть Х або О: "); string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || input.Trim().Length == 0)
                {
                ShowError("Невірний ввід! Виберіть Х або О.");
                Console.Clear();
                continue;
                }
                char choice = char.ToUpper(input.Trim()[0]);
                if (choice == 'X' || choice == 'Х')
                {
                    return 'X';
                }
                else if (choice == 'O' || choice == 'О' || choice == '0')
                {
                    return 'O';
                }
                else
                {
                    ShowError("Невірний ввід! Виберіть Х або О.");
                    Console.Clear();
                    continue;
                }
            }
            while (true);
        }
        static bool PlayerTurn(string playerName, char[,] board, char playerChoice)
        {
            int position = 0;
            while (true)
            {
                DrawBoard(board);
                Console.WriteLine($"{playerName}, ваш хід. Введіть номер вільного місця для {playerChoice}(Щоб вийти в головне меню натисніть 0): ");
                if (!int.TryParse(Console.ReadLine(), out int positionTest) || positionTest < 0 || positionTest > 9)
                {
                    ShowError("Невірний ввід! Введіть номер вільного місця від 1 до 9 або 0 для виходу в головне меню.");
                    Console.Clear();
                    continue;
                }
                position = positionTest;
                if (position == 0)
                {
                    Console.Clear();
                    return false;
                }
                if (board[(position - 1) / 3, (position - 1) % 3] == 'X' || board[(position - 1) / 3, (position - 1) % 3] == 'O')
                {
                    ShowError("Це місце вже зайняте! Виберіть інше вільне місце.");
                    Console.Clear();
                    continue;
                }
                break;
            }
            board[(position - 1) / 3, (position - 1) % 3] = playerChoice;
            return true;
        }
        static void PlayerBOT(string player2, char[,] board, char player2_choice)
        {
            Random random = new Random();
            Console.WriteLine($"Хід {player2}...");
            Thread.Sleep(1500);
            int row, col;
            do
            {
                row = random.Next(0, 3);
                col = random.Next(0, 3);
            }
            while (board[row, col] == 'X' || board[row, col] == 'O');
            board[row, col] = player2_choice;
        }
        static void DrawBoard(char[,] board)
        {
            Console.Clear();
            Console.WriteLine("Ігрове поле:\n");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i,j] + "\t");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        static bool CheckWin(char[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i,0] == board[i,1] && board[i,1] == board[i,2] && IsSymbol(board[i,0]))
                {
                    return true;
                }
                if (board[0,i] == board[1,i] && board[1,i] == board[2,i] && IsSymbol(board[0,i]))
                {
                    return true;
                } 
            }
            if (board[0,0] == board[1,1] && board[1,1] == board[2,2] && IsSymbol(board[0,0]))
                return true;
            else if (board[0,2] == board[1,1] && board[1,1] == board[2,0] && IsSymbol(board[0,2]))
                return true;
            return false;
        }
        static void ShowWinner(string winner)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Вітаємо! Переміг {winner}!");
            Console.ResetColor();
            Thread.Sleep(2000);
            Console.Clear();
        }
        static bool IsSymbol(char cell)
        {
            return cell == 'X' || cell == 'O';
        }
        static bool CheckDraw(char[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i,j] != 'X' && board[i, j] != 'O')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Thread.Sleep(2000);
        }
        static void Exit()
        {
            Console.WriteLine("Вихід з гри...");
            Thread.Sleep(1000);
            Console.Clear();
            Environment.Exit(0);
        }
    }
}