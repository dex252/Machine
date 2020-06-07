using System;
using System.Text;
using Machine.Example.MultiClassClassification;
using Machine.Example.MultiClassClassification.Models;

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

            var mcc = new MultiClassClassification();

            GitHubIssue issue = new GitHubIssue()
            {
                Title = "WebSockets communication is slow in my machine",
                Description = "The WebSockets communication used under the covers by SignalR looks like is going slow in my development machine.."
            };

            var prediction = mcc.Prediction(issue);
            Console.WriteLine(prediction);

            Console.ReadKey();
        }
    }
}
