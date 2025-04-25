namespace DBAudit.Infrastructure.Common.Command;

public class PrintMessage : IRequest
{
    public string Message { get; set; }
}

public class Add : IRequest<int>
{
    public int A { get; set; }
    public int B { get; set; }
}

public class AddHandler : ICommandHandler<Add, int>
{
    public Task<int> HandleAsync(Add request) => Task.FromResult(request.A + request.B);
}