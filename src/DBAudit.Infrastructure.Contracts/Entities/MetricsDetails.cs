namespace DBAudit.Infrastructure.Contracts.Entities;

public class ColumnMetrics
{
     public Guid Id { get; set; }
     public string Title { get; set; } = string.Empty;
     public string Value { get; set; }
     public Guid EnvironmentId { get; set; }
     public Guid DatabaseId { get; set; }
     public Guid TableId { get; set; }
     public Guid ColumnId { get; set; }
     public string Type { get; set; } = string.Empty;
     public DateTime CreatedAt { get; set; }
     public DateTime UpdatedAt { get; set; }



     public static ColumnMetrics Create(string title,
         string value,
         Guid environmentId,
         Guid databaseId,
         Guid tableId,
         Guid columnId,
         string type
     ) => new()
     {
         Id = Guid.NewGuid(),
         Title = title,
         Value = value,
         EnvironmentId = environmentId,
         DatabaseId = databaseId,
         TableId = tableId,
         ColumnId = columnId,
         Type = type,
         CreatedAt = DateTime.UtcNow,
         UpdatedAt = DateTime.UtcNow,
     };

}

public class TableMetrics
{
     public Guid Id { get; set; }
     public string Title { get; set; } = string.Empty;
     public string Value { get; set; }
     public Guid EnvironmentId { get; set; }
     public Guid DatabaseId { get; set; }
     public Guid TableId { get; set; }
     public string Type { get; set; } = string.Empty;
     public DateTime CreatedAt { get; set; }

     public DateTime UpdatedAt { get; set; }
     public static TableMetrics Create(string title,
         string value,
         Guid environmentId,
         Guid databaseId,
         Guid tableId,
         string type)
         => new TableMetrics()
         {
             Id = Guid.NewGuid(),
             Title = title,
             Value = value,
             EnvironmentId = environmentId,
             DatabaseId = databaseId,
             TableId = tableId,
             Type = type,
             CreatedAt = DateTime.UtcNow,
             UpdatedAt = DateTime.UtcNow,
         };
}


public class DatabaseMetrics
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid DatabaseId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    public static DatabaseMetrics Create(string title,
        string value,
        Guid environmentId,
        Guid databaseId,
        string type)
        => new ()
        {
            Id = Guid.NewGuid(),
            Title = title,
            Value = value,
            EnvironmentId = environmentId,
            DatabaseId = databaseId,
            Type = type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
}


public class EnvironmentMetrics
{
     public Guid Id { get; set; }
     public string Title { get; set; } = string.Empty;
     public string Value { get; set; }
     public Guid EnvironmentId { get; set; }
     public string Type { get; set; } = string.Empty;
     public DateTime CreatedAt { get; set; }
     
     public DateTime UpdatedAt { get; set; }
     public static EnvironmentMetrics Create(string title,
         string value,
         Guid environmentId,
         string type)
         => new ()
         {
             Id = Guid.NewGuid(),
             Title = title,
             Value = value,
             EnvironmentId = environmentId,
             Type = type,
             CreatedAt = DateTime.UtcNow,
             UpdatedAt = DateTime.UtcNow,
         };
}
