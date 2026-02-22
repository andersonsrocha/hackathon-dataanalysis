using HackathonDataAnalysis.Domain.Enums;

namespace HackathonDataAnalysis.Domain.Models;

public sealed class Rule : Entity
{
    public Rule() { }

    public Rule(Guid plotId, string name, SensorType sensorType, Operator @operator, double threshold, int durationMinutes, string status, string message)
    {
        Name = name;
        PlotId = plotId;
        SensorType = sensorType;
        Operator = @operator;
        Threshold = threshold;
        DurationMinutes = durationMinutes;
        Status = status;
        Message = message;
    }

    public Guid PlotId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public SensorType SensorType { get; set; } = SensorType.None;
    public Operator Operator { get; set; } = Operator.None;
    public double Threshold { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    public void Update(string name, SensorType sensorType, Operator @operator, double threshold, int durationMinutes, string status, string message)
    {
        Name = name;
        SensorType = sensorType;
        Operator = @operator;
        Threshold = threshold;
        DurationMinutes = durationMinutes;
        Status = status;
        Message = message;
    }
}