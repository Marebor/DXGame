namespace DXGame.Common.Helpers
{
    public interface IMapper
    {
        TDest Map<TSource, TDest>(TSource source);
    }
}