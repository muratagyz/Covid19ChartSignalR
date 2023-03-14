namespace Covid19ChartSignalR.API.Models;

public class CovidChart
{
    public CovidChart()
    {
        Count = new List<int>();
    }
    public string Date { get; set; }
    public List<int> Count { get; set; }
}