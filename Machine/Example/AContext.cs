using System;
using System.IO;
using Microsoft.ML;

namespace Machine.Example
{
    public abstract class AContext
    {
        protected MLContext mlContext;
        protected string ResourcesPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")) + @"Resources\";
    }
}