using System.IO;
using DXGame.Common.Infrastructure.Abstract;
using Polenter.Serialization;

namespace DXGame.Common.Infrastructure
{
    public class BinarySerializer : IBinarySerializer
    {
        
        public T Deserialize<T>(byte[] bin)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new SharpSerializer();
                stream.Read(bin, 0, bin.Length);
                return (T)serializer.Deserialize(stream);
            }
        }

        public byte[] Serialize<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new SharpSerializer();
                serializer.Serialize(obj, stream);
                return stream.ToArray();
            }
        }
    }
}