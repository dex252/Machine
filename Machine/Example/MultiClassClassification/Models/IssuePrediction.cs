using Microsoft.ML.Data;

namespace Machine.Example.MultiClassClassification.Models
{
    public class IssuePrediction
    {
        [ColumnName("PredictedLabel")]
        public string Area;
    }
}