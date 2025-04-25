namespace DBAudit.Infrastructure.Common.Command;

public class PrintMessage : ICommand
{
    public string Message { get; set; }
}