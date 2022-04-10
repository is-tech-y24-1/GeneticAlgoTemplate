// See https://aka.ms/new-console-template for more information

using GeneticAlgo.Shared;
using Serilog;

Logger.Init();
Log.Information("Start console polygon");
var dummyExecutionContext = new DummyExecutionContext();
dummyExecutionContext.OnRoundStart();
dummyExecutionContext.OnIterationStart();
dummyExecutionContext.OnRoundEnd();
Log.Information("Polygon end");

Console.WriteLine("Hello, World!");
