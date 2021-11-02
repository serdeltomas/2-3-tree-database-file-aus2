using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;

namespace Semka1
{
    public class App
    {
        private T23Tree<KeyStr, Osoba> _strOsobyPcrDatum = new T23Tree<KeyStr, Osoba>();
        private T23Tree<KeyDat, PcrTest> _strPcrDatum = new T23Tree<KeyDat, PcrTest>();
        private T23Tree<KeyInt, PcrTest> _strPcrID = new T23Tree<KeyInt, PcrTest>();
        private T23Tree<KeyInt, Miesto> _strKrajPcrDatum = new T23Tree<KeyInt, Miesto>();
        private T23Tree<KeyInt, Miesto> _strOkresPcrDatum = new T23Tree<KeyInt, Miesto>();
        private T23Tree<KeyInt, Miesto> _strPracoviskoPcrDatum = new T23Tree<KeyInt, Miesto>();
        private List<KeyInt> _pcrIdList = new List<KeyInt>();
        private List<KeyStr> _rodCisList = new List<KeyStr>();
        public App()
        {
        }
        public bool O01_VlozPCR(int pKodPracoviska, int pKodOkresu, int pKodKraja, string pRodCislo,
            DateTime pDatTestu, bool pVyslTestu, string pPoznamka)
        {
            //check if rod_cislo is in database
            if (!_strOsobyPcrDatum.Contains(new KeyStr(pRodCislo))) { return false; }
            //generate unique pcr ID
            var rand = new Random();
            int kodPcr = -1;
            do {
                kodPcr = rand.Next(1, Int32.MaxValue); //cisla id pcr testu od 1 po Int32.MaxValue
            } while (_pcrIdList.Contains(new KeyInt(kodPcr)));
            _pcrIdList.Add(new KeyInt(kodPcr));
            //create test and insert everywhere
            var pcrTest = new PcrTest(pKodPracoviska,kodPcr,pKodOkresu,pKodKraja,pRodCislo,pVyslTestu,pPoznamka);
            _strPcrDatum.Insert(new KeyDat(pDatTestu, kodPcr), pcrTest);
            _strPcrID.Insert(new KeyInt(kodPcr), pcrTest);
            _strOsobyPcrDatum.GetData(new KeyStr(pRodCislo)).GetTree().Insert(new KeyDat(pDatTestu, kodPcr), pcrTest);
            if (_strKrajPcrDatum.Contains(new KeyInt(pKodKraja))) 
                _strKrajPcrDatum.GetDataRef(new KeyInt(pKodKraja)).GetTree().Insert(new KeyDat(pDatTestu, kodPcr), pcrTest);
            else { _strKrajPcrDatum.Insert(new KeyInt(pKodKraja), new Miesto(pKodKraja));
                _strKrajPcrDatum.GetDataRef(new KeyInt(pKodKraja)).GetTree().Insert(new KeyDat(pDatTestu, kodPcr), pcrTest); }
            if (_strOkresPcrDatum.Contains(new KeyInt(pKodOkresu))) _strOkresPcrDatum.GetDataRef(
                new KeyInt(pKodOkresu)).GetTree().Insert(new KeyDat(pDatTestu, kodPcr), pcrTest);
            else { _strOkresPcrDatum.Insert(new KeyInt(pKodOkresu), new Miesto(pKodOkresu));
                _strOkresPcrDatum.GetDataRef(new KeyInt(pKodOkresu)).GetTree().Insert(new KeyDat(pDatTestu, kodPcr), pcrTest); }
            if (_strPracoviskoPcrDatum.Contains(new KeyInt(pKodPracoviska))) _strPracoviskoPcrDatum.GetDataRef(
                new KeyInt(pKodPracoviska)).GetTree().Insert(new KeyDat(pDatTestu, kodPcr), pcrTest);
            else { _strPracoviskoPcrDatum.Insert(new KeyInt(pKodPracoviska), new Miesto(pKodPracoviska));
                _strPracoviskoPcrDatum.GetDataRef(new KeyInt(pKodPracoviska)).GetTree().Insert(new KeyDat(pDatTestu, kodPcr), pcrTest); }
            return true;
        }
        public bool O17_VlozOsobu(string pMeno, string pPriezvisko, string pRodCislo, DateTime pDatNar)
        {
            if (_strOsobyPcrDatum.Insert(new KeyStr(pRodCislo), new Osoba(pMeno, pPriezvisko, pRodCislo, pDatNar))) 
            { _rodCisList.Add(new KeyStr(pRodCislo)); return true; }
            return false;

        }
        public bool GenerateData(int pNumOfPeople, int pNumOfTests)
        {
            if (_rodCisList.Count == 0 && pNumOfPeople == 0) return false;
            var rand = new Random();
            var firstNames = new string[]{
                "Noah" ,"Liam" ,"William" ,"Mason" , "James" ,"Benjamin" ,"Jacob" , "Michael" ,"Elijah" ,"Ethan" ,
                "Alexander" ,"Oliver" ,"Daniel" , "Lucas" ,"Matthew" ,"Aiden" , "Jackson" ,"Logan" ,"David" ,"Joseph" ,
                "Samuel" ,"Henry" ,"Owen" ,"Sebastian" , "Gabriel" ,"Carter" ,"Jayden" ,"John" , "Luke" ,"Anthony",
                "Olivia", "Emma", "Ava", "Charlotte", "Sophia", "Amelia", "Isabella", "Lucas", "Mia", "Henry", "Evelyn", "Harper"};
            var lastNames = new string[]{"Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez",
                "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson",
                "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
                "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green", " Adams",
                "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts" };
            for (int i = 0; i < pNumOfPeople; i++)
            {
                string cislotry;
                do {
                    cislotry = rand.Next(1, Int32.MaxValue).ToString("0000000000"); //rodne cisla od 1 po Int32.MaxValue
                } while (_rodCisList.Contains(new KeyStr(cislotry)));
                _rodCisList.Add(new KeyStr(cislotry));
                _strOsobyPcrDatum.Insert(new KeyStr(cislotry), new Osoba(firstNames[rand.Next(firstNames.Length)],
                    lastNames[rand.Next(lastNames.Length)], cislotry, rand.Next(1900, 2021),
                    rand.Next(1, 13), rand.Next(1, 29)));
            }
            for (int i = 0; i < pNumOfTests; i++)
            {
                int pcrID = 0;
                var pcrIDkey = new KeyInt(pcrID);
                do {
                    pcrID = rand.Next(1,Int32.MaxValue); //cisla id pcr testu od 1 po Int32.MaxValue
                    pcrIDkey = new KeyInt(pcrID);
                } while (_pcrIdList.Contains(pcrIDkey));
                _pcrIdList.Add(pcrIDkey);
                KeyStr aktRodCis = _rodCisList[rand.Next(_rodCisList.Count)];
                var aktDatum = new KeyDat(new DateTime(DateTime.Now.Ticks).AddDays(-rand.Next(600))
                    .AddHours(-rand.Next(24)).AddMinutes(-rand.Next(60)), pcrID);
                var prac = rand.Next(1, 151); // 1-150
                var pracKey = new KeyInt(prac);
                var okres = rand.Next(1, 80); // 1-79
                var okresKey = new KeyInt(okres);
                var kraj = rand.Next(1, 9); // 1-8
                var krajKey = new KeyInt(kraj);
                //insert everywhere
                var pcrTest = new PcrTest(prac, pcrID, okres, kraj,
                    aktRodCis.GetKey(), aktDatum.GetKey(), rand.Next(2)==0, "");
                _strOsobyPcrDatum.GetData(aktRodCis).GetTree().Insert(aktDatum, pcrTest);
                _strPcrDatum.Insert(aktDatum, pcrTest);
                _strPcrID.Insert(pcrIDkey, pcrTest);
                if (_strKrajPcrDatum.Contains(krajKey)) 
                    _strKrajPcrDatum.GetData(krajKey).GetTree().Insert(aktDatum, pcrTest);
                else
                {
                    _strKrajPcrDatum.Insert(krajKey, new Miesto(kraj));
                    _strKrajPcrDatum.GetData(krajKey).GetTree().Insert(aktDatum, pcrTest);
                }
                if (_strOkresPcrDatum.Contains(okresKey)) 
                    _strOkresPcrDatum.GetData(okresKey).GetTree().Insert(aktDatum, pcrTest);
                else
                {
                    _strOkresPcrDatum.Insert(okresKey, new Miesto(okres));
                    _strOkresPcrDatum.GetData(okresKey).GetTree().Insert(aktDatum, pcrTest);
                }
                if (_strPracoviskoPcrDatum.Contains(pracKey)) 
                    _strPracoviskoPcrDatum.GetData(pracKey).GetTree().Insert(aktDatum, pcrTest);
                else
                {
                    _strPracoviskoPcrDatum.Insert(pracKey, new Miesto(prac));
                    _strPracoviskoPcrDatum.GetData(pracKey).GetTree().Insert(aktDatum, pcrTest);
                }
            }
            return true;
        }
        public bool O02_VyhladajTest(int pCisloTestu, string pRodCislo, ref string textOut)
        {
            var testNaVypis = _strPcrID.GetData(new KeyInt(pCisloTestu));
            var osobaNaVypis = _strOsobyPcrDatum.GetData(new KeyStr(pRodCislo));
            if (testNaVypis == null || osobaNaVypis == null) return false;
            textOut = testNaVypis.ToString() + " " + osobaNaVypis.ToString();
            return true;
        }
        public string VypisVsetko()
        {
            if (_strOsobyPcrDatum == null && _strPcrID == null)
                return "";
            if (_strOsobyPcrDatum == null)
                return _strPcrID.VypisVsetko();
            if (_strPcrID == null)
                return _strOsobyPcrDatum.VypisVsetko();
            return _strOsobyPcrDatum.VypisVsetko() + _strPcrID.VypisVsetko();
        }
        public bool O04_VypisPozitivnychOkresDatum(int numOkresu, DateTime datOd, DateTime datDo, ref string vypis)
        {
            var okr = new KeyInt(numOkresu);
            if (!_strOkresPcrDatum.Contains(okr)) return false;
            var datOdKey = new KeyDat(datOd, 1);
            var datDoKey = new KeyDat(datDo, Int32.MaxValue);
            var zoznam = _strOkresPcrDatum.GetData(okr).GetTree().InOrder(datOdKey, datDoKey);
            var zozNeg = new List<PcrTest>();
            foreach (var z in zoznam)
            {
                if (z.IsPositive()) zozNeg.Add(z);
            }
            vypis += "pocet: " + zozNeg.Count + "\n";
            foreach (var z in zozNeg)
            {
                vypis += z.ToString();
            }
            return true;
        }
        public bool O05_VypisVsetkychOkresDatum(int numOkresu, DateTime datOd, DateTime datDo, ref string vypis)
        {
            var okr = new KeyInt(numOkresu);
            if (!_strOkresPcrDatum.Contains(okr)) return false;
            var datOdKey = new KeyDat(datOd, 1);
            var datDoKey = new KeyDat(datDo, Int32.MaxValue);
            var zoznam = _strOkresPcrDatum.GetData(okr).GetTree().InOrder(datOdKey, datDoKey);
            vypis += "pocet: " + zoznam.Count + "\n";
            foreach (var z in zoznam)
            {
                vypis += z.ToString();
            }
            return true;
        }
        public bool O15_VypisVsetkychPracDatum(int numPracoviska, DateTime datOd, DateTime datDo, ref string vypis)
        {
            var pracKey = new KeyInt(numPracoviska);
            if (!_strPracoviskoPcrDatum.Contains(pracKey)) return false;
            var datOdKey = new KeyDat(datOd, 1);
            var datDoKey = new KeyDat(datDo, Int32.MaxValue);
            var zoznam = _strPracoviskoPcrDatum.GetData(pracKey).GetTree().InOrder(datOdKey, datDoKey);
            vypis += "pocet: " + zoznam.Count + "\n";
            foreach (var z in zoznam)
            {
                vypis += z.ToString();
            }
            return true;
        }

    }
}