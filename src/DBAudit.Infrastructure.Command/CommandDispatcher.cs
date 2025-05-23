using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Command;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService(handlerType);
        dynamic handler = service;
        
        return await handler.HandleAsync((dynamic)request);
    }

    public async Task Send<TRequest>(TRequest request) where TRequest : IRequest
    {
        var requestType = request.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(requestType);
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService(handlerType);
        dynamic handler = service;
        
        await handler.HandleAsync((dynamic)request);
    }
}