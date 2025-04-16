namespace DBAudit.Infrastructure.Data.Entities;

public class Table
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid DatabaseId { get; set; }
}