using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Do you want to enter new word? yes(y) or no(n)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Y)
                {
                    Console.Write("\nYour Word: ");
                    string newWord = Console.ReadLine();
                    List<string> words = new List<string>();
                    
                    using (StreamReader sr = new StreamReader("dict.txt"))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            words.Add(line);
                        }
                    }
                    if (!words.Contains(newWord))
                    {
                        using (StreamWriter sw = new StreamWriter("dict.txt", true))
                        {
                            sw.WriteLine(newWord);
                        }
                    }
                    else
                    {
                        Console.WriteLine("This word exists in the dictionary");
                    }
                }
                else
                {
                    break;
                }
            }            
        }
    }
}
