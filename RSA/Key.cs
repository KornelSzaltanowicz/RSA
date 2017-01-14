using System;

namespace RSA
{
    [Serializable]
    public abstract class Key
    {
        public int N { get; set; }

        protected Key(int n)
        {
            N = n;
        }
    }

    [Serializable]
    public class PrivateKey : Key
    {
        public int D { get; private set; }
        //public int N => base.N;

        public PrivateKey(int n, int d) : base(n)
        {
            D = d;
        }
    }

    [Serializable]
    public class PublicKey : Key
    {
        public int E { get; private set; }

        public PublicKey(int n, int e) : base(n)
        {
            E = e;
        }
    }
}
