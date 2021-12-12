using System;

namespace Semka2
{
    class Osoba : ICsv, IDataToFIle<Osoba>
    {
        private string _meno;
        private string _priezvisko;
        private string _rodCislo;
        private DateTime _datNar;

        private const int MENO_MAX_LEN = 15;
        private const int PRIE_MAX_LEN = 20;
        private const int RC_MAX_LEN = 10;
        private const int DAT_MAX_LEN = 25;
        private const int FULL_MAX_LEN = 3 * sizeof(Int32) + (MENO_MAX_LEN + PRIE_MAX_LEN + RC_MAX_LEN + DAT_MAX_LEN + 1) * sizeof(char);

        private BTree<KeyDat, PcrTest> _stromPcr = new BTree<KeyDat, PcrTest>(); //not taking tree into account atm

        public Osoba() { }
        public Osoba(string pMeno, string pPriezvisko, string pRodCislo, int pRokNar, int pMesNar, int pDenNar)
        {
            _meno = pMeno;
            _priezvisko = pPriezvisko;
            _rodCislo = pRodCislo;
            _datNar = new DateTime(pRokNar, pMesNar, pDenNar);
        }
        public Osoba(string pMeno, string pPriezvisko, string pRodCislo, DateTime pDatNar)
        {
            _meno = pMeno;
            _priezvisko = pPriezvisko;
            _rodCislo = pRodCislo;
            _datNar = pDatNar;
        }
        public override string ToString()
        {
            //return "  rod. cislo: " + _rodCislo + "   dat. narodenia: " + _datNar + "\t meno: " + _meno + "\t priezvisko: " + _priezvisko + "\n";
            return _rodCislo;
        }
        public string GetMeno() {return _meno; }
        public string GetPriezv() { return _priezvisko; }
        public string GetRodCislo()                     
        {   
            return _rodCislo;
        }
        public BTree<KeyDat, PcrTest> GetTree()
        {
            return _stromPcr;
        }
        public string ToStringCsv()
        {
            return _rodCislo + ";" + _datNar + ";" + _meno + ";" + _priezvisko;
        }
        public byte[] ToByteArray()
        {
            var rt = new byte[0];
            //meno
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_meno.Length));
            var a1 = _meno.PadLeft(MENO_MAX_LEN);
            foreach (var ch in a1) { rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(ch)); }
            //priezv
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_priezvisko.Length));
            var a2 = _priezvisko.PadLeft(PRIE_MAX_LEN);
            foreach (var ch in a2) { rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(ch)); }
            //rod cis
            foreach (var ch in _rodCislo) { rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(ch)); }
            //dat nar
            var datNar = _datNar.ToString();
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(datNar.Length));
            var a3 = datNar.PadLeft(DAT_MAX_LEN);
            foreach (var ch in a3) { rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(ch)); }
            //strom somewhere herre

            //newline
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes('\n'));
            return rt;
        }
        public Osoba FromByteArray(byte[] pArray)
        {
            //meno
            var a1 = "";
            for (int i = 0; i < MENO_MAX_LEN; i++)
                if (i >= MENO_MAX_LEN - BitConverter.ToInt32(pArray, 0))
                    a1 = a1 + BitConverter.ToChar(pArray, sizeof(Int32) + i * sizeof(char));
            //priezv
            var a2 = "";
            for (int i = 0; i < PRIE_MAX_LEN; i++)
                if (i >= PRIE_MAX_LEN - BitConverter.ToInt32(pArray, sizeof(Int32) + sizeof(char) * MENO_MAX_LEN))
                    a2 = a2 + BitConverter.ToChar(pArray, 2*sizeof(Int32) + sizeof(char) * MENO_MAX_LEN + i * sizeof(char));
            //rod cis
            var a3 = "";
            for (int i = 0; i < RC_MAX_LEN; i++)
                a3 = a3 + BitConverter.ToChar(pArray, 2 * sizeof(Int32) + sizeof(char) * (MENO_MAX_LEN + PRIE_MAX_LEN + i));
            //dat nar
            var a4 = "";
            for (int i = 0; i < DAT_MAX_LEN; i++)
                if (i >= DAT_MAX_LEN - BitConverter.ToInt32(pArray, 2 * sizeof(Int32) + sizeof(char) * (MENO_MAX_LEN + PRIE_MAX_LEN+ RC_MAX_LEN)))
                    a4 = a4 + BitConverter.ToChar(pArray, 3 * sizeof(Int32) + sizeof(char) * (MENO_MAX_LEN + PRIE_MAX_LEN + RC_MAX_LEN + i));
            //strom somewhero hero

            //return
            return new Osoba(a1, a2, a3, DateTime.Parse(a4));
        }
        public int Size()
        {
            return FULL_MAX_LEN;
        }

        public string StringFromFIle()
        {
            return _rodCislo;
        }
    }
}