using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Helpers
{
    public static class SerializeHelper
    {

        public static MessageHelper Serialize(object anySerializableObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, anySerializableObject);
                return new MessageHelper { Data = memoryStream.ToArray() };
            }
        }

        public static object Deserialize(MessageHelper message)
        {
            using (var memoryStream = new MemoryStream(message.Data))
                return (new BinaryFormatter()).Deserialize(memoryStream);
        }

    }
}
