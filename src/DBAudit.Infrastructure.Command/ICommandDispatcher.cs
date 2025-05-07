namespace DBAudit.Infrastructure.Command;

public interface ICommandDispatcher
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);

    Task Send<TRequest>(TRequest request) where TRequest : IRequest;
}