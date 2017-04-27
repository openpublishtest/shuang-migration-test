using System;

namespace CSharpDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            // <snippet6>
            var maturities = new[] { 1, 2, 5, 10 };
            var analyzers = RateAnalysis.Analyzer.Analyzer.GetAnalyzers(maturities);

            foreach (var item in analyzers)
            {
                Console.WriteLine("Min = {0}, \t Max = {1}, \t Curent = {2}", item.Min, item.Max, item.Current);
            }
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
            // </snippet6>
        }
    }
}