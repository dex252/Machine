using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class AnalyzesSentiment
    {
        private static string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")) + @"Machine\Resources\";
        Machine.Example.AnalyzesSentiment.AnalyzesSentiment analyzesSentiment = new Machine.Example.AnalyzesSentiment.AnalyzesSentiment(path);
        [TestMethod]
        public void TestPrediction1()
        {
            var prediction = analyzesSentiment.Prediction("I love this coffee!");

            Assert.AreEqual(prediction, true);
        }
        [TestMethod]
        public void TestPrediction2()
        {
            var prediction = analyzesSentiment.Prediction("This was a horrible meal");

            Assert.AreEqual(prediction, false);
        }
        [TestMethod]
        public void TestPrediction3()
        {
            var prediction = analyzesSentiment.Prediction("I love this spaghetti.");

            Assert.AreEqual(prediction, true);
        }
        [TestMethod]
        public void TestPrediction4()
        {
            var prediction = analyzesSentiment.Prediction("This cook can't cook");

            Assert.AreEqual(prediction, false);
        }

        [TestMethod]
        public void TestBulkPrediction()
        {
           
            var prediction = analyzesSentiment.BulkPrediction("This is bad wine", "This is a great wine!", "What a delicious sandwich!?");

            CollectionAssert.AreEqual(prediction.ToList(), new bool[]{false, true, true}.ToList());
        }
    }
}