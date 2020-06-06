using Microsoft.ML.Data;

namespace Machine.Example.LinearRegression.Models
{
    public class Prediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}