namespace DXGame.Common.Services
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string str);
    }
}