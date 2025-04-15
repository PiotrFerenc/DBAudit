namespace DBAudit.Web.Data.Entities;

public class Environment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}