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
        private BTree<KeyDat, PcrTest> _stromPcr = new BTree<KeyDat, PcrTest>();
        private BTree<KeyRodCis, PcrTest> _stromPcrRodCislo = new BTree<KeyRodCis, PcrTest>();
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

        public BTree<KeyDat, PcrTest> GetTree()
        {
            return _stromPcr;
        }
        public BTree<KeyRodCis, PcrTest> GetTreeRodCis()
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

        public string StringFromFIle()
        {
            throw new NotImplementedException();
        }
    }
    
}
