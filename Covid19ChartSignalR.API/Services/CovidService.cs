using Covid19ChartSignalR.API.Context;
using Covid19ChartSignalR.API.Hubs;
using Covid19ChartSignalR.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Covid19ChartSignalR.API.Services;

public class CovidService
{
    private readonly AppDbContext _context;
    private readonly IHubContext<CovidHub> _hubContext;

    public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public IQueryable<Covid> GetList()
    {
        return _context.Covids.AsQueryable();
    }

    public async Task SaveCovid(Covid covid)
    {
        await _context.Covids.AddAsync(covid);
        await _context.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidList());
    }

    public List<CovidChart> GetCovidList()
    {
        List<CovidChart> covidCharts = new List<CovidChart>();
        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText =
                "SELECT to_char(\"CovidDate\", 'DD/MM/YYYY')," +
                " round(avg((CASE WHEN \"City\" = 1 THEN \"Count\" END)), 2) AS \"1\"," +
                " round(avg((CASE WHEN \"City\" = 2 THEN \"Count\" END)), 2) AS \"2\"," +
                " round(avg((CASE WHEN \"City\" = 3 THEN \"Count\" END)), 2) AS \"3\"," +
                " round(avg((CASE WHEN \"City\" = 4 THEN \"Count\" END)), 2) AS \"4\"," +
                " round(avg((CASE WHEN \"City\" = 5 THEN \"Count\" END)), 2) AS \"5\"" +
                " FROM public.\"Covids\"" +
                " GROUP BY to_char(\"CovidDate\", 'DD/MM/YYYY')" +
                " ORDER BY to_char(\"CovidDate\", 'DD/MM/YYYY') asc;";

            command.CommandType = System.Data.CommandType.Text;

            _context.Database.OpenConnection();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CovidChart c = new CovidChart();

                    c.Date = reader.GetString(0);

                    Enumerable.Range(1, 5).ToList().ForEach(x =>
                    {
                        if (System.DBNull.Value.Equals(reader[x]))
                        {
                            c.Count.Add(0);
                        }
                        else
                        {
                            c.Count.Add(reader.GetInt32(x));
                        }
                    });

                    covidCharts.Add(c);
                }
            }

            _context.Database.CloseConnection();

            return covidCharts;
        }
    }
}