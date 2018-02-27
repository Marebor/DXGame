using System.Threading.Tasks;

namespace DXGame.Messages.Commands
{
    public interface ICommandHandler<T> where T : ICommand 
    {
        Task HandleAsync(T command); 
    }
}