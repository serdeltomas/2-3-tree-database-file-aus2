#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;

namespace Semka1
{
    class PcrTest
    {
        private int _kodPracoviska;
        private int _kodPcr;
        private int _kodOkresu;
        private int _kodKraja;
        private string _rodCislo;
        private DateTime _datTestu;
        private bool _vyslTestu;
        private string _poznamka;
        
        //konštruktor so zadavanim datumu
        public PcrTest(int pKodPracoviska,int pKodPcr,int pKodOkresu, int pKodKraja, string pRodCislo,
            DateTime pDatTestu, bool pVyslTestu, string pPoznamka)
        {
            _kodPracoviska = pKodPracoviska;
            _kodPcr = pKodPcr;
            _kodOkresu = pKodOkresu;
            _kodKraja = pKodKraja;
            _rodCislo = pRodCislo;
            _datTestu = pDatTestu;
            _vyslTestu = pVyslTestu;
            _poznamka = pPoznamka;
        }
        
        public PcrTest(int pKodPracoviska,int pKodPcr,int pKodOkresu, int pKodKraja, string pRodCislo,
            bool pVyslTestu, string pPoznamka)
        {
            _kodPracoviska = pKodPracoviska;
            _kodPcr = pKodPcr;
            _kodOkresu = pKodOkresu;
            _kodKraja = pKodKraja;
            _rodCislo = pRodCislo;
            _datTestu = DateTime.Now;
            _vyslTestu = pVyslTestu;
            _poznamka = pPoznamka;
        }

        public override string ToString()
        {
            return "  kod pcr: " + _kodPcr.ToString("0000000000") + " rod. cislo: " + _rodCislo + 
                " kraj: " + _kodKraja + " okres: " + _kodOkresu.ToString("00") + " prac: " + _kodPracoviska.ToString("000") + 
                " dat. testu: " + _datTestu + "\t pozitivny: " + _vyslTestu + "\t poznamka: " + _poznamka + "\n";
        }
        public bool IsPositive()
        {
            return _vyslTestu;
        }
    }

    
}