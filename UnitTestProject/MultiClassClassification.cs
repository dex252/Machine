using System;
using System.IO;
using Machine.Example.MultiClassClassification.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class MultiClassClassification
    {
        private static string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")) + @"Machine\Resources\";
        Machine.Example.MultiClassClassification.MultiClassClassification multiClassClassification = new Machine.Example.MultiClassClassification.MultiClassClassification(path);

        [TestMethod]
        public void TestPrediction1()
        {
            GitHubIssue issue = new GitHubIssue()
            {
                Title = "WebSockets communication is slow in my machine",
                Description = "The WebSockets communication used under the covers by SignalR looks like is going slow in my development machine.."
            };

           var prediction =  multiClassClassification.Prediction(issue);

           Assert.AreEqual(prediction, "area-System.Net");

        }
        [TestMethod]
        public void TestPrediction2()
        {
            GitHubIssue issue = new GitHubIssue()
            {
                Title = "Directory.CreateDirectory(directory) call when the directory already exists ends up in a StackOverflowException",
                Description = "Directory.CreateDirectory(directory) call when the directory already exists ends up in a StackOverflowException. It used to fail silently in System.IO."
            };

            var prediction = multiClassClassification.Prediction(issue);

            Assert.AreEqual(prediction, "area-System.IO");

        }
        [TestMethod]
        public void TestPrediction3()
        {
            GitHubIssue issue = new GitHubIssue()
            {
                Title = "Proposal: Add Task.WhenAny overloads with CancellationToke",
                Description = "It's a relatively common pattern to await on tasks which don't support cancellation (yet). One workaround is to pass new Task(() => {}, cancellationToken) as one of the tasks to Task.WhenAny, but I've also seen a variant where the token registration sets a TCS result and the TCS.Task is passed to WhenAny and even more complicated variants, but all of them are pretty inefficient and make the code unwieldy.\r\nThere are somewhat similar WaitAny overloads for sync-over-async code already."
            };

            var prediction = multiClassClassification.Prediction(issue);

            Assert.AreEqual(prediction, "area-System.Threading");

        }
        [TestMethod]
        public void TestPrediction4()
        {
            GitHubIssue issue = new GitHubIssue()
            {
                Title = "Allow X509Chain callers to prevent retrieval of missing certificates",
                Description = "Callers sometimes want to build a \"fully offline\" X509Chain, but the only exposed online/offline mode pertains to revocation processing. Alternatively, they may want to be in a \"limited online\" state, where the only network operations are for retrieving up-to-date CRL/OCSP responses for the end-entity certificate."
            };

            var prediction = multiClassClassification.Prediction(issue);

            Assert.AreEqual(prediction, "area-System.Security");

        }
    }
}