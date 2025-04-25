namespace DBAudit.Infrastructure.Common.Command;

public interface ICommandHandler<in T> where T : IRequest
{
    Task HandleAsync(T command);
}

public interface ICommandHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
}