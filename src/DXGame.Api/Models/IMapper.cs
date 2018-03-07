namespace DXGame.Api.Models
{
    public interface IMapper
    {
        T Map<T>(object from);
    }
}