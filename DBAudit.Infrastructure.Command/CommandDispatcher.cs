using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Command;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var service = serviceProvider.GetRequiredService(handlerType);
        dynamic handler = service;
        
        return handler.HandleAsync((dynamic)request);
    }

    public Task Send<TRequest>(TRequest request) where TRequest : IRequest
    {
        var requestType = request.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(requestType);
        var service = serviceProvider.GetRequiredService(handlerType);
        dynamic handler = service;
        
        return handler.HandleAsync((dynamic)request);
    }
}