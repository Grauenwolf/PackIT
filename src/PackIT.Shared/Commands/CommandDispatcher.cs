using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PackIT.Shared.Commands
{
    public sealed class CommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public CommandDispatcher(IServiceProvider serviceProvider, ILogger<CommandDispatcher> logger)

        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var commandType = command.GetType().Name;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();


                _logger.LogInformation($"Started processing {commandType} command.");
                await handler.HandleAsync(command);
                _logger.LogInformation($"Finished processing {commandType} command.");

            }
            catch
            {
                _logger.LogError($"Failed to process {commandType} command.");
                throw;
            }
        }
    }
}
