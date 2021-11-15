#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;

namespace Semka1
{
    class PcrTest: ICsv
    {
        private int _kodPracoviska;
        private int _kodPcr;
        private int _kodOkresu;
        private int _kodKraja;
        private string _rodCislo;
        private DateTime _datTestu;
        private bool _vyslTestu;
        private string _poznamka;
        private Osoba _osoba;
        
        //konštruktor so zadavanim datumu
        public PcrTest(int pKodPracoviska,int pKodPcr,int pKodOkresu, int pKodKraja, string pRodCislo, Osoba pOsoba,
            DateTime pDatTestu, bool pVyslTestu, string pPoznamka)
        {
            _kodPracoviska = pKodPracoviska;
            _kodPcr = pKodPcr;
            _kodOkresu = pKodOkresu;
            _kodKraja = pKodKraja;
            _rodCislo = pRodCislo;
            _osoba = pOsoba;
            _datTestu = pDatTestu;
            _vyslTestu = pVyslTestu;
            _poznamka = pPoznamka;
        }
        
        public PcrTest(int pKodPracoviska,int pKodPcr,int pKodOkresu, int pKodKraja, string pRodCislo, Osoba pOsoba,
            bool pVyslTestu, string pPoznamka)
        {
            _kodPracoviska = pKodPracoviska;
            _kodPcr = pKodPcr;
            _kodOkresu = pKodOkresu;
            _kodKraja = pKodKraja;
            _rodCislo = pRodCislo;
            _datTestu = DateTime.Now;
            _osoba = pOsoba;
            _vyslTestu = pVyslTestu;
            _poznamka = pPoznamka;
        }

        public override string ToString()
        {
            return "  kod pcr: " + _kodPcr.ToString("0000000000") + "   kraj: " + _kodKraja + "  okres: " + _kodOkresu.ToString("00") 
                + "   prac: " + _kodPracoviska.ToString("000") +  "   dat. testu: " + _datTestu + "\t pozitivny: " + _vyslTestu 
                + "\t poznamka: " + _poznamka + "\t rod. cislo: " + _rodCislo + "\t meno: " + _osoba.GetMeno() + "\t priezvisko: " + _osoba.GetPriezv() + "\n";
        }
        public bool IsPositive()
        {
            return _vyslTestu;
        }
        public int GetKod() { return _kodPcr; }
        public int GetKraj() { return _kodKraja; }
        public int GetOkres() { return _kodOkresu; }
        public int GetPrac() { return _kodPracoviska; }
        public DateTime GetDat() { return _datTestu; }
        public string GetRodCis() { return _rodCislo; }

        public string ToStringCsv()
        {
            return _kodPcr + ";" + _kodKraja + ";" + _kodOkresu + ";" + _kodPracoviska + ";" 
                + _datTestu + ";" + _vyslTestu + ";" + _poznamka + ";" + _rodCislo;
        }
    }

    
}