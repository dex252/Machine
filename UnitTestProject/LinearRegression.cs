using Machine.Example.LinearRegression.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class LinearRegression
    {
        static HouseData[] houseData = {
            new HouseData() { Size = 1.1F, Price = 1.2F },
            new HouseData() { Size = 1.9F, Price = 2.3F },
            new HouseData() { Size = 2.8F, Price = 3.0F },
            new HouseData() { Size = 3.4F, Price = 3.7F }
        };

        static HouseData[] testHouseData =
        {
            new HouseData() { Size = 1.1F, Price = 0.98F },
            new HouseData() { Size = 1.9F, Price = 2.1F },
            new HouseData() { Size = 2.8F, Price = 2.9F },
            new HouseData() { Size = 3.4F, Price = 3.6F }
        };
        Machine.Example.LinearRegression.LinearRegression linear = new Machine.Example.LinearRegression.LinearRegression(houseData, testHouseData);
        [TestMethod]
        public void TestPrediction1()
        {
            var prediction = linear.Prediction(new HouseData() { Size = 12f });

            Assert.IsTrue(Delta());

            bool Delta()
            {
                return (prediction > 1_100_000 && prediction < 1_300_000);
            }
        }

        [TestMethod]
        public void TestPrediction2()
        {
            var prediction = linear.Prediction(new HouseData() { Size = 3f });

            Assert.IsTrue(Delta());

            bool Delta()
            {
                return (prediction > 300_000 && prediction < 350_000);
            }
        }

        [TestMethod]
        public void TestEvaluateR2()
        {
            var r2 = linear.EvaluateR2();
            Assert.IsTrue(Delta());

            bool Delta()
            {
                return (r2 > 0.7 && r2 <= 1);
            }
        }

        [TestMethod]
        public void TestEvaluateRms()
        {
            var rms = linear.EvaluateRMS();

            Assert.IsTrue(Delta());

            bool Delta()
            {
                return (0.20 - rms > 0);
            }
        }
    }
}
