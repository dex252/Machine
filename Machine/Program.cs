using System;
using System.Text;
using Machine.Example.AnalyzesSentiment;

namespace Machine 
{
    /// <summary>
    /// Класс для промежуточных тестов
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            AnalyzesSentiment ass = new AnalyzesSentiment();
            ass.Prediction("I hate it!");
            ass.Prediction("This was a horrible meal");
            ass.Prediction("I love this spaghetti.");
            ass.Prediction("This cook can't cook");

            Console.ReadKey();
        }
    }
}
