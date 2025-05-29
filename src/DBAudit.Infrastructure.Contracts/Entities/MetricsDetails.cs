namespace DBAudit.Infrastructure.Contracts.Entities;

public class Metric
{
     public Guid Id { get; set; }
     public string Title { get; set; } = string.Empty;
     public string Value { get; set; } = string.Empty;
     public string Key { get; set; } = string.Empty;
     public string Type { get; set; } = string.Empty;
     public DateTime CreatedAt { get; set; }
     public DateTime UpdatedAt { get; set; }

     public static Metric Create(string title,
         string value,
         string type,
         MetricKey key
     ) => new()
     {
         Id = Guid.NewGuid(),
         Title = title,
         Value = value,
         Type = type,
         CreatedAt = DateTime.UtcNow,
         UpdatedAt = DateTime.UtcNow,
         Key = key.Key,
     };
}

public record ColumnName(string Value)
{
    public static ColumnName Empty() => new ColumnName(String.Empty);
};
public record TableName(string Value);
public record DatabaseName(string Value);
public record EnvName(string Value);

public class MetricKey
{
    public string Key { get; private set; }
    private readonly char _separator = ':';
    public MetricKey(ColumnName columnName)
    {
        Key = columnName.Value; 
    }
    public MetricKey(  EnvName envName)
    {
        Key = $"{envName.Value}"; 
    }
    public MetricKey( DatabaseName databaseName, EnvName envName)
    {
        Key = $"{databaseName.Value}{_separator}{envName.Value}"; 
    }
    public MetricKey( TableName tableName, DatabaseName databaseName, EnvName envName)
    {
        Key = $"{tableName.Value}{_separator}{databaseName.Value}{_separator}{envName.Value}"; 
    }
    public MetricKey(ColumnName columnName, TableName tableName, DatabaseName databaseName, EnvName envName)
    {
        Key = $"{columnName.Value}{_separator}{tableName.Value}{_separator}{databaseName.Value}{_separator}{envName.Value}"; 
    }
}