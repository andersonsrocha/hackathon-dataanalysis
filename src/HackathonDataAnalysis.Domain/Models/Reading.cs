namespace HackathonDataAnalysis.Domain.Models;

public sealed class Reading : Entity
{
    public Reading() { }

    public Reading(Guid plotId, string plotName, DateTime date, double soilMoisture, double temperature, double precipitation)
    {
        PlotId = plotId;
        PlotName = plotName;
        Date = date;
        SoilMoisture = soilMoisture;
        Temperature = temperature;
        Precipitation = precipitation;
    }

    public Guid PlotId { get; set; } = Guid.Empty;
    public string PlotName { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public double SoilMoisture { get; set; } = 0.0;
    public double Temperature { get; set; } = 0.0;
    public double Precipitation { get; set; } = 0.0;
    
    public void Update(DateTime date, double soilMoisture, double temperature, double precipitation)
    {
        Date = date;
        SoilMoisture = soilMoisture;
        Temperature = temperature;
        Precipitation = precipitation;
    }
}