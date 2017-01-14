using System;
using System.Collections.Generic;

namespace RSA
{
    public class EncryptionRsa : RSACipher
    {
        public int E{ get; set; }
        private PublicKey Key{ get; }

        private readonly List<int> _codedMessageLetters = new List<int>();

        public EncryptionRsa(Key publicKey)
        {
            Key = publicKey as PublicKey;
        }

        private void ConvertMessageToDigits(string message)
        {
            foreach (var character in message)
            {
                _codedMessageLetters.Add(character);
            }
        }

        public IEnumerable<double> Encrypt(string message)
        {
            var result = new List<double>();
            double c;
            ConvertMessageToDigits(message);


            _codedMessageLetters.ForEach(asciiLetter =>
            {
                c = Math.Pow(asciiLetter, Key.E) % Key.N;
                result.Add(c);
            });

            return result;
        }

    }
}
