using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Machine.Example.AnalyzesSentiment.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Machine.Example.AnalyzesSentiment
{
    public class AnalyzesSentiment : AContext
    {
        private ITransformer model;
        private CalibratedBinaryClassificationMetrics metrics;
        public AnalyzesSentiment()
        {
            DataOperationsCatalog.TrainTestData splitDataView = LoadData();
            BuildAndTrainModel(splitDataView.TrainSet);
            Evaluate(splitDataView.TestSet);
        }
        /// <summary>
        /// Точность. Чем ближе к 1.00, тем лучше.
        /// https://docs.microsoft.com/ru-ru/dotnet/machine-learning/resources/metrics
        /// </summary>
        /// <returns></returns>
        public double Accuracy()
        {
            var accuracy = metrics.Accuracy;
            Console.WriteLine($"Accuracy: {accuracy:P2}");

            return accuracy;
        }

        /// <summary>
        /// Площадь под кривой. Чем ближе к 1,00, тем лучше.
        /// https://docs.microsoft.com/ru-ru/dotnet/machine-learning/resources/metrics
        /// </summary>
        /// <returns></returns>
        public double Auc()
        {
            var auc = metrics.AreaUnderRocCurve;
            Console.WriteLine($"Auc: {auc:P2}");

            return auc;
        }

        /// <summary>
        /// Cбалансированная F-оценка. Чем ближе к 1,00, тем лучше.
        /// https://docs.microsoft.com/ru-ru/dotnet/machine-learning/resources/metrics
        /// </summary>
        /// <returns></returns>
        public double F1Score()
        {
            var score = metrics.F1Score;
            Console.WriteLine($"F1Score: {score:P2}");

            return score;
        }
        /// <summary>
        /// Прогнозирование, на входе - комментарий
        /// </summary>
        /// <param name="CommentText"></param>
        public bool Prediction(string CommentText)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
            var sampleStatement = new SentimentData
            {
                SentimentText = CommentText
            };
            var resultPrediction = predictionFunction.Predict(sampleStatement);
            
            Console.WriteLine($@"Sentiment: {resultPrediction.SentimentText} | Prediction: {(Convert.ToBoolean(resultPrediction.Prediction) ? "Positive" : "Negative")} | Probability: {resultPrediction.Probability} ");

            return resultPrediction.Prediction;
        }

        /// <summary>
        /// Прогнозирование, на входе массив комментариев
        /// https://docs.microsoft.com/ru-ru/dotnet/machine-learning/tutorials/sentiment-analysis
        /// </summary>
        /// <param name="comments"></param>
        /// <returns></returns>
        public bool[] BulkPrediction(params string[] comments)
        {
            List<SentimentData> sentiments = new List<SentimentData>();
            foreach (var c in comments)
            {
                sentiments.Add(new SentimentData {SentimentText = c});
            }

            IDataView batchComments = mlContext.Data.LoadFromEnumerable(sentiments);
            IDataView predictions = model.Transform(batchComments);
            IEnumerable<SentimentPrediction> predictedResults = mlContext.Data.CreateEnumerable<SentimentPrediction>(predictions, reuseRowObject: false);

            bool[] results = new bool[predictedResults.Count()];
            int count = 0;
            foreach (SentimentPrediction prediction in predictedResults)
            {
                Console.WriteLine($"Sentiment: {prediction.SentimentText} | Prediction: {(Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative")} | Probability: {prediction.Probability} ");
                results[count] = prediction.Prediction;
                count++;
            }

            return results;
        }

        private void Evaluate(IDataView splitTestSet)
        {
            IDataView predictions = model.Transform(splitTestSet);
            metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");
        }

        private void BuildAndTrainModel(IDataView splitTrainSet)
        {
            var estimator = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features",
                inputColumnName: nameof(SentimentData.SentimentText))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
            model = estimator.Fit(splitTrainSet);
        }

        /// <summary>
        /// Загрузка набора данных и его разделение на набор для обучения и тестовый набор
        /// 20% - тестовый набор, константа
        /// </summary>
        /// <returns></returns>
        private DataOperationsCatalog.TrainTestData LoadData()
        {
            File.WriteAllText(TempPathToData, Machine.Example.AnalyzesSentiment.Resource.yelp_labelled);

            IDataView dataView = mlContext.Data.LoadFromTextFile<SentimentData>(TempPathToData, hasHeader: false);

            DataOperationsCatalog.TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            return splitDataView;
        }
    }
}