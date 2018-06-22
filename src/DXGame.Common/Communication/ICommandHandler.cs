using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Communication
{
    public interface ICommandHandler<T> : IMessageHandler<T> where T : ICommand 
    {
    
    }
}