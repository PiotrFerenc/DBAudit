using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Common.Command;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var service = serviceProvider.GetRequiredService(handlerType);
        dynamic handler = service;
        
        return handler.HandleAsync((dynamic)request);
    }

    public Task Send<TRequest>(TRequest request) where TRequest : IRequest
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(request.GetType());
        var service = serviceProvider.GetRequiredService(handlerType);
        dynamic handler = service;
        
        return handler.HandleAsync((dynamic)request);
    }
}