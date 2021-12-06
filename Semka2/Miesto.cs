using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semka2
{
    class Miesto : ICsv, IDataToFIle<Miesto>
    {
        private int _cislo;
        private T23Tree<KeyDat, PcrTest> _stromPcr = new T23Tree<KeyDat, PcrTest>();
        private T23Tree<KeyRodCis, PcrTest> _stromPcrRodCislo = new T23Tree<KeyRodCis, PcrTest>();
        private const int FULL_MAX_LEN = 0;

        public Miesto() { }
        public Miesto(int pCislo)
        {
            _cislo = pCislo;
        }
        public Miesto FromByteArray(byte[] pArray)
        {
            throw new NotImplementedException();
        }

        public T23Tree<KeyDat, PcrTest> GetTree()
        {
            return _stromPcr;
        }
        public T23Tree<KeyRodCis, PcrTest> GetTreeRodCis()
        {
            return _stromPcrRodCislo;
        }

        public int Size()
        {
            return FULL_MAX_LEN;
        }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public string ToStringCsv()
        {
            return _cislo.ToString();
        }
    }
    
}
