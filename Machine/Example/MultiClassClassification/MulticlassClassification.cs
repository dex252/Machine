using System;
using Machine.Example.MultiClassClassification.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Machine.Example.MultiClassClassification
{
    /// <summary>
    /// https://docs.microsoft.com/ru-ru/dotnet/machine-learning/tutorials/github-issue-classification
    /// </summary>
    public class MultiClassClassification: AContext
    {
        private ITransformer model;
        private CalibratedBinaryClassificationMetrics metrics;
        private PredictionEngine<GitHubIssue, IssuePrediction> predEngine;
        public MultiClassClassification()
        {
            mlContext = new MLContext(seed: 0);

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<GitHubIssue>(ResourcesPath + "issues_test.tsv", hasHeader: true);
            var pipeline = ProcessData();
            BuildAndTrainModel(trainingDataView, pipeline);
            predEngine = mlContext.Model.CreatePredictionEngine<GitHubIssue, IssuePrediction>(model);
        }

        public string Prediction(GitHubIssue issue)
        {
            var prediction = predEngine.Predict(issue);
            Console.WriteLine($"=============== Single Prediction just-trained-model - Result: {prediction.Area} ===============");
            return prediction.Area;
        }

        private void BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            var trainingPipeline = pipeline.Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                                           .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            model = trainingPipeline.Fit(trainingDataView);
        }

        private IEstimator<ITransformer> ProcessData()
        {
            return mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Area", outputColumnName: "Label")
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title", outputColumnName: "TitleFeaturized"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Description", outputColumnName: "DescriptionFeaturized"))
                .Append(mlContext.Transforms.Concatenate("Features", "TitleFeaturized", "DescriptionFeaturized"))
                .AppendCacheCheckpoint(mlContext); // для кеширование небольших и средних объемов данных
        }
    }
}