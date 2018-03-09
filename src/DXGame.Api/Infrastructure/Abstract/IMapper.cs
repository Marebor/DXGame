namespace DXGame.Api.Infrastructure.Abstract
{
    public interface IMapper
    {
        T Map<T>(object from);
    }
}