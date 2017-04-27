module DemoRates

let treasuryMaturities = [ 1; 2; 5; 10 ]

/// The URLs for downloading historical data on treasury securities is available from the
/// federal reserve web site.  URLs have the form:
/// http://www.federalreserve.gov/releases/h15/data/business_day/H15_TCMNOM_Yn.txt
/// where n is the number of years to maturity.

let analyzers = RateAnalysis.Analyzer.Analyzer.GetAnalyzers(treasuryMaturities);

let maturitiesAndAnalyzers = Seq.zip treasuryMaturities analyzers

printfn "Historical Treasury bill nominal rates, compared."
for maturity, analyzer in maturitiesAndAnalyzers do
    printfn "Data for treasury bill with %d year maturity." maturity
    printfn "Min Rate =\t %5.2f %% on\t %s\nMax Rate =\t %5.2f %% on\t %10s\n" (fst analyzer.Min) (snd analyzer.Min)
     (fst analyzer.Max) (snd analyzer.Max)
    printfn "Current Rate=\t %5.2f %%" analyzer.Current



