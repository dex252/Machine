using System;
using System.Text;
using Machine.Example.LinearRegression.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace Machine.Example.LinearRegression
{
    /// <summary>
    /// https://docs.microsoft.com/ru-ru/dotnet/machine-learning/how-does-mldotnet-work
    /// </summary>
    public class LinearRegression: AContext
    {
        // 1. Import or create training data
        private HouseData[] houseData;

        private HouseData[] testHouseData;

        private TransformerChain<RegressionPredictionTransformer<LinearRegressionModelParameters>> model;
        private RegressionMetrics metrics;
        public LinearRegression(HouseData[] houseData)
        {
            mlContext = new MLContext();
            this.houseData = houseData;
            Learn();
        }

        public LinearRegression(HouseData[] houseData, HouseData[] testHouseData)
        {
            mlContext = new MLContext();
            this.houseData = houseData;
            this.testHouseData = testHouseData;
            Learn();
            Metrics();
        }

        private void Learn()
        {
            Console.OutputEncoding = Encoding.UTF8;
            IDataView trainingData = mlContext.Data.LoadFromEnumerable(houseData);

            // 2. Specify data preparation and model training pipeline
            var pipeline = mlContext.Transforms.Concatenate("Features",
                    new[] { "Size" })
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Price", maximumNumberOfIterations: 100));

            // 3. Train model
            model = pipeline.Fit(trainingData);
        }

        private void Metrics()
        {
            var testHouseDataView = mlContext.Data.LoadFromEnumerable(testHouseData);
            var testPriceDataView = model.Transform(testHouseDataView);

            metrics = mlContext.Regression.Evaluate(testPriceDataView, labelColumnName: "Price");
        }

        public float Prediction(HouseData inputData)
        {
            var size = inputData;
            var price = mlContext.Model.CreatePredictionEngine<HouseData, Prediction>(model).Predict(size);
            
            Console.WriteLine($"Predicted price for size: {size.Size * 1000} = {price.Price * 100_000:C}");

            return price.Price*100_000;
        }
        /// <summary>
        /// R-квадрат, коэффициент детерминации. Чем ближе к 1.00, тем выше качество.
        /// https://docs.microsoft.com/ru-ru/dotnet/machine-learning/resources/metrics
        /// </summary>
        /// <returns></returns>
        public double EvaluateR2()
        {
            if (testHouseData != null)
            {
                var rSquared = metrics.RSquared;
                Console.WriteLine($"R^2: {rSquared:0.##}");
                return rSquared;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Среднеквадратичное отклонение. Значения ближе к 0.00 предпочтительнее.
        /// https://docs.microsoft.com/ru-ru/dotnet/machine-learning/resources/metrics
        /// </summary>
        /// <returns></returns>
        public double EvaluateRMS()
        {
            if (testHouseData != null)
            {
                var rootMeanSquaredError = metrics.RootMeanSquaredError;
                Console.WriteLine($"RMS error: {rootMeanSquaredError:0.##}");
                return rootMeanSquaredError;
            }

            throw new NotImplementedException();
        }
    }
}