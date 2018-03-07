using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Api.Models
{
    public interface IBroadcaster
    {
        Task BroadcastAsync(IEvent e);
    }
}