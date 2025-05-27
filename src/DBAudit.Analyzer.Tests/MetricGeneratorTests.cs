using DBAudit.Application.Feature.Analyze;
using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Analyzer.Tests;

public class MetricGeneratorTests
{
    [Fact]
    public void CreateMetricForTable()
    {
        var envId = Guid.NewGuid();
        var dbId = Guid.NewGuid();
        var tableId = Guid.NewGuid();
        const string type1 = "test_type";
        const string type2 = "some_type";

        var columnMetrics = new List<ColumnMetrics>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty,
                Value = 1,
                EnvironmentId = envId,
                DatabaseId = dbId,
                TableId = tableId,
                ColumnId = Guid.NewGuid(),
                Type = type1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty,
                Value = 1,
                EnvironmentId = envId,
                DatabaseId = dbId,
                TableId = tableId,
                ColumnId = Guid.NewGuid(),
                Type = type1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty,
                Value = 1,
                EnvironmentId = envId,
                DatabaseId = dbId,
                TableId = tableId,
                ColumnId = Guid.NewGuid(),
                Type = type2
            }
        };

        var result = MetricsGenerator.For(columnMetrics, metric => TableMetrics.Create(metric.Key, metric.Value, envId, dbId, tableId, metric.Key));
        
        Assert.Equal(2, result.Count);
        var first = result.First();
        var last = result.Last();

        Assert.Equal(type1, first.Type);
        Assert.Equal(2, first.Value);
        Assert.Equal(dbId, first.DatabaseId);
        Assert.Equal(envId, first.EnvironmentId);

        Assert.Equal(type2, last.Type);
        Assert.Equal(1, last.Value);
        Assert.Equal(dbId, last.DatabaseId);
        Assert.Equal(envId, last.EnvironmentId);
    }
}