using System.Threading.Tasks;

namespace DXGame.Common.Messages.Commands
{
    public interface ICommandHandler<T> where T : ICommand 
    {
        Task HandleAsync(T command); 
    }
}