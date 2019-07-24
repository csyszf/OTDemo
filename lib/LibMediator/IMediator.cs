using System.Threading.Tasks;
using LibMediator.Command;

namespace LibMediator
{
    public interface IMediator
    {
        Task<CommandResult> Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
