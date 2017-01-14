using System.Collections.Generic;

namespace RSA
{
    public class DecryptionRSA : RSACipher
    {
        private PrivateKey key{ get; set; }
        private int resultT{ get; set; }
        
        public DecryptionRSA(Key privateKey)
        {
            key = privateKey as PrivateKey;
        }

        public List<double> Decrypt(IEnumerable<double> messageList)
        {
            var result = new List<double>();
            foreach (var item in messageList)
            {
                result.Add(DecodeBlock(item));
            }
            //var decodedLetters =  messageList.Select(DecodeBlock).ToList();
            //decodedLetters.ForEach(letter => result += letter);
            return result;
        }

        private int DecodeBlock(double block)
        {
            resultT = power_modulo_fast(block, key.D, key.N);
            
            return resultT;
        }

        //Metoda szybkiego potęgowania zaczerpnięta ze strony: http://www.algorytm.org/algorytmy-arytmetyczne/szybkie-potegowanie-modularne/spm-cs.html
        //// www.algorytm.org
        // (c)2007 Tomasz Lubinski
        private static int power_modulo_fast(double a, int b, int m)
        {
            int i;
            double result = 1;
            double x = a % m;

            for (i = 1; i <= b; i <<= 1)
            {
                x %= m;
                if ((b & i) != 0)
                {
                    result *= x;
                    result %= m;
                }
                x *= x;
            }

            return (int)result;
        }

    }
}
