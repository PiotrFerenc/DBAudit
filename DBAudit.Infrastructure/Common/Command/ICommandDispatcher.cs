namespace DBAudit.Infrastructure.Common.Command;

public interface ICommandDispatcher
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);

    Task Send<TRequest>(TRequest request) where TRequest : IRequest;
}