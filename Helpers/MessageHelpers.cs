using System;
using RSA;

namespace Helpers
{
    public class MessageHelper
    {
        public byte[] Data { get; set; }
    }

    [Serializable]
    public class MessageObject
    {
        public MessageObject(Key key)
        {
            Key = key;
        }

        public Key Key { get; private set; }
        
    }
}
