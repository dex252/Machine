using System.IO;
using Microsoft.ML;

namespace Machine.Example
{
    public abstract class AContext
    {
        protected readonly MLContext mlContext = new MLContext();
        protected string TempPathToData = Path.GetTempPath() + "TempDataML";
    }
}