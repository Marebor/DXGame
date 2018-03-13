namespace DXGame.ReadModel.Infrastructure.Abstract
{
    public interface IMapper
    {
        T Map<T>(object from);
    }
}