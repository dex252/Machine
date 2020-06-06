using System;
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
        readonly HouseData[] houseData = {
            new HouseData() { Size = 1.1F, Price = 1.2F },
            new HouseData() { Size = 1.9F, Price = 2.3F },
            new HouseData() { Size = 2.8F, Price = 3.0F },
            new HouseData() { Size = 3.4F, Price = 3.7F }
        };

        private TransformerChain<RegressionPredictionTransformer<LinearRegressionModelParameters>> model;

        public LinearRegression()
        {
            IDataView trainingData = mlContext.Data.LoadFromEnumerable(houseData);

            // 2. Specify data preparation and model training pipeline
            var pipeline = mlContext.Transforms.Concatenate("Features",
                    new[] { "Size" })
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Price", maximumNumberOfIterations: 100));

            // 3. Train model
            model = pipeline.Fit(trainingData);
        }

        public void Prediction(HouseData inputData)
        {
            var size = inputData;
            var price = mlContext.Model.CreatePredictionEngine<HouseData, Prediction>(model).Predict(size);
           
            Console.WriteLine($"Predicted price for size: {size.Size * 1000} = {price.Price * 100_000:C}");
        }
    }
}