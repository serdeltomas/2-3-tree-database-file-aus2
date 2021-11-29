using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semka2
{
    class Miesto : ICsv
    {
        private int _cislo;//not used
        private T23Tree<KeyDat, PcrTest> _stromPcr = new T23Tree<KeyDat, PcrTest>();
        private T23Tree<KeyRodCis, PcrTest> _stromPcrRodCislo = new T23Tree<KeyRodCis, PcrTest>();

        public Miesto() { }
        public Miesto(int pCislo)
        {
            _cislo = pCislo;
        }
        public int CompareTo(Miesto? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return _cislo.CompareTo(other._cislo);
        }
        public T23Tree<KeyDat, PcrTest> GetTree()
        {
            return _stromPcr;
        }
        public T23Tree<KeyRodCis, PcrTest> GetTreeRodCis()
        {
            return _stromPcrRodCislo;
        }

        public string ToStringCsv()
        {
            return _cislo.ToString();
        }
    }
    
}
