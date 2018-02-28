namespace DXGame.Common.Services
{
    public interface IMapper
    {
        TDest Map<TSource, TDest>(TSource source);
    }
}