using System.Threading.Tasks;

namespace LibMediator.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<CommandResult> Handle(TCommand command);
    }
}
