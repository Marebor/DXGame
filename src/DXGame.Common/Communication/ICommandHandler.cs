using System.Threading.Tasks;

namespace DXGame.Common.Communication
{
    public interface ICommandHandler<T> where T : ICommand 
    {
        Task HandleAsync(T command); 
    }
}