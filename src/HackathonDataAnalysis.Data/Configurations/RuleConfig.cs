using HackathonDataAnalysis.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackathonDataAnalysis.Data.Configurations;

public class RuleConfig : EntityConfig<Rule>
{
    protected override void Map(EntityTypeBuilder<Rule> builder)
    {
        builder.Property(p => p.Operator)
            .HasConversion<int>();
        
        builder.Property(p => p.SensorType)
            .HasConversion<int>();

        builder.Property(p => p.Threshold)
            .HasPrecision(18, 2);
    }
}