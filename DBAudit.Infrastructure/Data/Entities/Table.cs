namespace DBAudit.Infrastructure.Data.Entities;

public class Table
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid DatabaseId { get; set; }
}