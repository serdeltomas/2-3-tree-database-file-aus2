using System;

namespace Semka1
{
    class Osoba
    {
        private string _meno;
        private string _priezvisko;
        private string _rodCislo;
        private DateTime _datNar;
        private T23Tree<KeyDat, PcrTest> _stromPcr = new T23Tree<KeyDat, PcrTest>();

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

        /*public override string ToString()
        {
            return _rodCislo;
        } */
        public override string ToString()
        {
            return "  rod. cislo: " + _rodCislo + " dat. narodenia: " + _datNar + "\t meno: " + _meno + "\t priezvisko: " + _priezvisko + "\n";
        }
        
        
        public string GetMeno()
        {
            return _meno;}

        public string GetRodCislo()                     
        {   
            return _rodCislo;
        }
        public T23Tree<KeyDat, PcrTest> GetTree()
        {
            return _stromPcr;
        }
    }
}