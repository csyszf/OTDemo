using System;
using System.Threading.Tasks;
using LibMediator.Command;
using Microsoft.Extensions.DependencyInjection;

namespace LibMediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<CommandResult> Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            ICommandHandler<TCommand> handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            return handler.Handle(command);
        }
    }
}
