using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);
 var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal:0.##}{Environment.NewLine}");

IEnumerable<string> FindFiles(string foldername)
{
    List<string> salesFiles = new List<string>();
    var foundFiles = Directory.EnumerateFiles(foldername, "*", SearchOption.AllDirectories);
    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }
    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    foreach( var file in salesFiles)
    {
        var salesJson = File.ReadAllText(file);
        SalesData? salesData = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        salesTotal += salesData?.Total ?? 0;
    }

    return salesTotal;
}
record SalesData(double Total);

class SalesTotal
{
    public double Total { get; set; }
}
