namespace DXGame.Common.Infrastructure.Abstract
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T obj);
        T Deserialize<T>(byte[] bin);
    }
}