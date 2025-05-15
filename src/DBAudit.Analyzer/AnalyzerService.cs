namespace DBAudit.Analyzer;

public record TableId(Guid Value)
{
    public static TableId Empty => new(Guid.Empty);
    public static TableId Create(Guid id) => new(id);
};

public record DbId(Guid Value)
{
    public static DbId Empty => new(Guid.Empty);
    public static DbId Create(Guid id) => new(id);
};

public record EnvId(Guid Value)
{
    public static EnvId Empty => new(Guid.Empty);
    public static EnvId Create(Guid id) => new(id);
};