using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class AnalyzesSentiment
    {
        [TestMethod]
        public void TestPrediction1()
        {
            var analyzesSentiment = new Machine.Example.AnalyzesSentiment.AnalyzesSentiment();

            var prediction = analyzesSentiment.Prediction("I love this coffee!");

            Assert.AreEqual(prediction, true);
        }
        [TestMethod]
        public void TestPrediction2()
        {
            var analyzesSentiment = new Machine.Example.AnalyzesSentiment.AnalyzesSentiment();

            var prediction = analyzesSentiment.Prediction("This was a horrible meal");

            Assert.AreEqual(prediction, false);
        }
        [TestMethod]
        public void TestPrediction3()
        {
            var analyzesSentiment = new Machine.Example.AnalyzesSentiment.AnalyzesSentiment();

            var prediction = analyzesSentiment.Prediction("I love this spaghetti.");

            Assert.AreEqual(prediction, true);
        }
        [TestMethod]
        public void TestPrediction4()
        {
            var analyzesSentiment = new Machine.Example.AnalyzesSentiment.AnalyzesSentiment();

            var prediction = analyzesSentiment.Prediction("This cook can't cook");

            Assert.AreEqual(prediction, false);
        }

        [TestMethod]
        public void TestBulkPrediction()
        {
            var analyzesSentiment = new Machine.Example.AnalyzesSentiment.AnalyzesSentiment();

            var prediction = analyzesSentiment.BulkPrediction("This is bad wine", "This is a great wine!", "What a delicious sandwich!?");

            Assert.AreEqual(prediction, new []{false, true, true});
        }
    }
}