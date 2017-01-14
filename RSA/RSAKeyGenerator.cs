using System;
using System.Collections.Generic;
using System.Numerics;

namespace RSA
{
    public static class RSAKeyGenerator
    {

        private static int p;
        private static int q;
        private static BigInteger Fi;
        private static int modulN;
        private static int wykladnikPublicznyE;
        private static int wykladnikPrywatnyD;

        public static Key PrivateKey => new PrivateKey(modulN, wykladnikPrywatnyD);
        public static Key PublicKey => new PublicKey(modulN, wykladnikPublicznyE);

        static RSAKeyGenerator()
        {
            Init();
        }

        private static void Init()
        {
            var primaNumbers = GeneratePrimeNumbers();
            p = primaNumbers[0];
            q = primaNumbers[1];
            Fi = CalculateFi(p, q);
            modulN = CalculateN(p, q);
            wykladnikPublicznyE = CalculateE(Fi);
            wykladnikPrywatnyD = CalculateD(Fi, wykladnikPublicznyE);
        }

        private static List<int> GeneratePrimeNumbers()
        {

            var primeNumbers = new List<int>();

            for (var i = 100; i < 200; i++) 
            {
                var primeNumber = true;

                for (uint n = 2; n < i; n++)
                {
                    if (i % n == 0)
                    {
                        primeNumber = false;
                        break;
                    }
                }
                if (primeNumber)
                {
                    primeNumbers.Add(i);
                }

            }
            var random = new Random();
            var result = new List<int>();
            for (var i = 0; i < 2; i++)
            {
                result.Add(primeNumbers[random.Next(0, primeNumbers.Count)]);
            }

            return result;
        }

        private static BigInteger CalculateFi(int p, int q) => (p - 1) * (q - 1);
        private static int CalculateN(int p, int q) => p * q;
        private static BigInteger CalculateNWD(BigInteger p, BigInteger q)
        {
            while (p != q)
            {
                if (p > q)
                    p -= q;
                else
                    q -= p;
            }
            return p;
        }

        private static int CalculateE(BigInteger Fi)
        {
            int result = 1;
            for (int i = 2; i < Fi; i++)
            {
                if (CalculateNWD(Fi, i) == 1)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        private static int CalculateD(BigInteger Fi, int e)
        {
            int result = 1;

            for (int i = 2; i < Fi; i++)
            {
                if ((i * e) % Fi == 1)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
    }

    
}
