﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semka2;

namespace Semka2GUI
{
    public partial class MainWindow : Form
    {

        private App _app = new App();
        public MainWindow()
        {
            InitializeComponent();
            osoba_datum_narodenia.Value = DateTime.Now;
            chori_datum.Value = DateTime.Now;
            pcr_datum_testu.Value = DateTime.Now;
            hladaj_dat_do.Value = DateTime.Now;
            hladaj_dat_od.Value = DateTime.Now.AddDays(-5);
        }

        private void btn17_Click(object sender, EventArgs e)
        {
            //vloz osobu
            if (osoba_meno.Text == "" || osoba_priezvisko.Text == "" || osoba_rod_cislo.Text == "") // not checking date of birth
            {
                osoba_success.Text = "vyplnte vsetky udaje";
                return;
            }
            if(osoba_rod_cislo.Text.Length != 10)
            {
                osoba_success.Text = "nespravny format rodneho cisla";
                return;
            }
            try
            {
                if (_app.O17_VlozOsobu(osoba_meno.Text, osoba_priezvisko.Text,
                    osoba_rod_cislo.Text, osoba_datum_narodenia.Value))
                {
                    osoba_success.Text = "uspesne vlozena";
                    count_all.Text = _app.Pocty();
                    return;
                }
                else osoba_success.Text = "nepodarilo sa";
            }
            catch(DuplicateWaitObjectException)
            {
                osoba_success.Text = "osoba uz je v systeme";
            }
            catch (NullReferenceException)
            {
                osoba_success.Text = "vyplnte vsetky udaje";
            }
        }

        private void btn01_Click(object sender, EventArgs e)
        {//vloz pcr test
            if (pcr_kraj.Value == 0 || pcr_okres.Value == 0 || pcr_pracovisko.Value == 0 || pcr_rod_cislo.Text == "")
            {
                pcr_success.Text = "vyplnte vsetky udaje";
                return;
            }
            if (pcr_rod_cislo.Text.Length != 10)
            {
                pcr_success.Text = "nespravny format rodneho cisla";
                return;
            }
            try
            {
                if (_app.O01_VlozPCR((int)pcr_pracovisko.Value, (int)pcr_okres.Value, (int)pcr_kraj.Value,
                    pcr_rod_cislo.Text, pcr_datum_testu.Value, pcr_positive.Checked, pcr_poznamka.Text))
                {
                    pcr_success.Text = "uspesne vlozene";
                    count_all.Text = _app.Pocty();
                    return;
                }
                else pcr_success.Text = "osoba nie je v systeme";
            }
            catch (NullReferenceException)
            {
                pcr_success.Text = "vyplnte vsetky udaje";
            }
        }

        private void btn_gen_Click(object sender, EventArgs e)
        {
            gen_success.Text = "generujem ....";
            if (!_app.GenerateData((int)gen_osoby.Value, (int)gen_testy.Value)) { gen_success.Text = "ziadne osoby v databaze"; return; }
            gen_success.Text = "uspesne vygenerovane " + gen_osoby.Value + " osob a " + gen_testy.Value + " testov";
            count_all.Text = _app.Pocty();
            return;
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (o02_pcr_id.Value == 0)
            {
                o02_success.Text = "vyplnte aspon udaj 'Cislo PCR testu'";
                return;
            }
            if (o02_rod_cislo.Text != "" && o02_rod_cislo.Text.Length != 10)
            {
                o02_success.Text = "nespravny format rodneho cisla";
                return;
            }
            if (o02_rod_cislo.Text != "") {
                string strOut = "";
                if (!_app.O02_VyhladajTest((int)o02_pcr_id.Value, o02_rod_cislo.Text, ref strOut))
                {
                    o02_success.Text = "dany pcr test alebo osoba nie je v databaze"; return;
                }
                o02_success.Text = "vypis je v novom okne";
                var f = new WindowOut(strOut);
                f.Text = "Vyhladanie testu";
                f.Show(); return;
            }
            else
            {
                string strOut = "";
                if (!_app.O16_VyhladajTest((int)o02_pcr_id.Value, ref strOut))
                {
                    o02_success.Text = "dany pcr test nie je v databaze"; return;
                }
                o02_success.Text = "vypis je v novom okne";
                var f = new WindowOut(strOut);
                f.Text = "Vyhladanie testu";
                f.Show(); return;
            }
        }

        private void btn_dummy_Click(object sender, EventArgs e)
        {
            //_app.GenerateData(1000000, 1000000);
            //var f = new WindowOut("DONE");
            //f.Text = "Files saved";
            //f.Show();
        }

        private void btn_vypis_vsetko_Click(object sender, EventArgs e)
        {
            if (o03_rod_cislo.Text == "")
            {
                o03_success.Text = "vypis je v novom okne";
                var f = new WindowOut(_app.VypisVsetko());
                f.Text = "Vypis vsetkych udajov v databaze";
                f.Show();
                return;
            }
            if (o03_rod_cislo.Text.Length != 10)
            {
                o03_success.Text = "nespravny format rodneho cisla";
                return;
            }
            var vypisText = "";
            if (!_app.O03_VypisVsetkyPcrOsoba(o03_rod_cislo.Text, ref vypisText))
            {
                o03_success.Text = "osoba nebola najdena v zozname";
                return;
            }
            else
            {
                o03_success.Text = "vypis je v novom okne";
                var f = new WindowOut(vypisText);
                f.Text = "Vypis vsetkych testov danej osoby";
                f.Show();
                return;
            }
        }

        private void btn_hladaj_testy_datum_Click(object sender, EventArgs e)
        {
            if ((hladaj_kraj.Value != 0 && hladaj_okres.Value != 0 && hladaj_prac.Value == 0) ||
                (hladaj_kraj.Value != 0 && hladaj_okres.Value == 0 && hladaj_prac.Value != 0) ||
                (hladaj_kraj.Value == 0 && hladaj_okres.Value != 0 && hladaj_prac.Value != 0))
            {
                hladaj_success.Text = "vyplnte ziaden alebo prave jeden z udajov(kraj/okres/pracovisko)";
                return;
            }
            var hladajDatOd = new DateTime(hladaj_dat_od.Value.Year, hladaj_dat_od.Value.Month, hladaj_dat_od.Value.Day);
            var hladajDatDo = new DateTime(hladaj_dat_do.Value.Year, hladaj_dat_do.Value.Month, hladaj_dat_do.Value.Day + 1);
            if (hladajDatOd < hladajDatDo)
            {
                hladaj_success.Text = "datum do je mensi ako datum od";
                return;
            }
            if (hladaj_kraj.Value == 0 && hladaj_okres.Value == 0 && hladaj_prac.Value == 0)
            {
                if (hladaj_iba_pozitivne.Checked)
                {
                    string vypis = "";
                    if (!_app.O08_VypisVsetkychPozitivnychTestovDatum(hladajDatOd, hladajDatDo, ref vypis))
                    {
                        hladaj_success.Text = "miesto ktore ste zadali nie je v databaze";
                        return;
                    }
                    hladaj_success.Text = "vypis je v novom okne";
                    var f = new WindowOut(vypis);
                    f.Text = "Vypis vsetkych pozitivnych testov za zadane casove obdobie";
                    f.Show();
                    return;
                }
                else
                {
                    string vypis = "";
                    if (!_app.O09_VypisVsetkychTestovDatum(hladajDatOd, hladajDatDo, ref vypis))
                    {
                        hladaj_success.Text = "miesto ktore ste zadali nie je v databaze";
                        return;
                    }
                    hladaj_success.Text = "vypis je v novom okne";
                    var f = new WindowOut(vypis);
                    f.Text = "Vypis vsetkych testov za zadane casove obdobie";
                    f.Show();
                    return;
                }
            }
            if (hladaj_okres.Value != 0 && hladaj_iba_pozitivne.Checked)
            {
                string vypis = "";
                if (!_app.O04_VypisPozitivnychOkresDatum((int)hladaj_okres.Value, hladajDatOd, hladajDatDo, ref vypis))
                {
                    hladaj_success.Text = "miesto ktore ste zadali nie je v databaze";
                    return;
                }
                hladaj_success.Text = "vypis je v novom okne";
                var f = new WindowOut(vypis);
                f.Text = "Vypis pozitivnych testov z vybraneho okresu za zadane casove obdobie";
                f.Show();
                return;
            }
            if (hladaj_okres.Value != 0 && !hladaj_iba_pozitivne.Checked)
            {
                string vypis = "";
                if (!_app.O05_VypisVsetkychOkresDatum((int)hladaj_okres.Value, hladajDatOd, hladajDatDo, ref vypis))
                {
                    hladaj_success.Text = "miesto ktore ste zadali nie je v databaze";
                    return;
                }
                hladaj_success.Text = "vypis je v novom okne";
                var f = new WindowOut(vypis);
                f.Text = "Vypis vsetkych testov z vybraneho okresu za zadane casove obdobie";
                f.Show();
                return;
            }
            if (hladaj_kraj.Value != 0 && hladaj_iba_pozitivne.Checked)
            {
                string vypis = "";
                if (!_app.O06_VypisPozitivnychKrajDatum((int)hladaj_kraj.Value, hladajDatOd, hladajDatDo, ref vypis))
                {
                    hladaj_success.Text = "miesto ktore ste zadali nie je v databaze";
                    return;
                }
                hladaj_success.Text = "vypis je v novom okne";
                var f = new WindowOut(vypis);
                f.Text = "Vypis pozitivnych testov z vybraneho kraja za zadane casove obdobie";
                f.Show();
                return;
            }
            if (hladaj_kraj.Value != 0 && !hladaj_iba_pozitivne.Checked)
            {
                string vypis = "";
                if (!_app.O07_VypisVsetkychKrajDatum((int)hladaj_kraj.Value, hladajDatOd, hladajDatDo, ref vypis))
                {
                    hladaj_success.Text = "miesto ktore ste zadali nie je v databaze";
                    return;
                }
                hladaj_success.Text = "vypis je v novom okne";
                var f = new WindowOut(vypis);
                f.Text = "Vypis vsetkych testov z vybraneho kraja za zadane casove obdobie";
                f.Show();
                return;
            }
            if (hladaj_prac.Value != 0 && !hladaj_iba_pozitivne.Checked)
            {
                string vypis = "";
                if (!_app.O15_VypisVsetkychPracDatum((int)hladaj_prac.Value, hladajDatOd, hladajDatDo, ref vypis))
                {
                    hladaj_success.Text = "miesto ktore ste zadali nie je v databaze";
                    return;
                }
                hladaj_success.Text = "vypis je v novom okne";
                var f = new WindowOut(vypis);
                f.Text = "Vypis vsetkych testov z vybraneho pracoviska za zadane casove obdobie";
                f.Show();
                return;
            }
            if (hladaj_prac.Value != 0 && hladaj_iba_pozitivne.Checked)
            {
                hladaj_success.Text = "funkcionalita nie je podporovana";
            }
        }

        private void btn19_Click(object sender, EventArgs e)
        {//vymaz osobu
            if (vymaz_rod_cislo.Text == "")
            {
                vymaz_success.Text = "vyplnte rodne cislo";
                return;
            }
            if (vymaz_rod_cislo.Text.Length != 10)
            {
                vymaz_success.Text = "nespravny format rodneho cisla";
                return;
            }
            if (!_app.O19_VymazOsobu(vymaz_rod_cislo.Text))
            {
                vymaz_success.Text = "zadane rodne cislo nie je v databaze";
                return;
            }
            vymaz_success.Text = "zadane rodne cislo a vsetky jeho testy boli odstranene z databazy";
            count_all.Text = _app.Pocty();
            return;
        }

        private void btn18_Click(object sender, EventArgs e)
        {
            if (vymaz_pcr_id.Text == "")
            {
                vymaz_success.Text = "vyplnte cislo pcr testu";
                return;
            }
            if (!_app.O18_VymazPcrTest((int)(vymaz_pcr_id.Value)))
            {
                vymaz_success.Text = "zadane cislo testu nie je v databaze";
                return;
            }
            vymaz_success.Text = "zadane cislo testu bolo odstranene z databazy";
            count_all.Text = _app.Pocty();
            return;
        }

        private void btn_file_save_Click(object sender, EventArgs e)
        {
            _app.SaveToFiles();
            file_success.Text = "uspesne ulozene do suboru"; 
        }

        private void btn_file_read_Click(object sender, EventArgs e)
        {
            _app.ReadFromFiles();
            file_success.Text = "uspesne nacitane zo suboru";
            count_all.Text = _app.Pocty();
        }

        private void btn_chori_Click(object sender, EventArgs e)
        {
            var datumDo = new DateTime(chori_datum.Value.Year, chori_datum.Value.Month, chori_datum.Value.Day);
            if (chori_kraj.Value != 0 && chori_okres.Value != 0 )
            {
                chori_success.Text = "vyplnte ziaden alebo prave jeden z udajov(kraj/okres)";
                return;
            }
            if(chori_kraj.Value == 0 && chori_okres.Value == 0)
            {
                string vypis = "";
                if (!_app.O12_VypisVsetkychChorych((int)(chori_pocet_dni.Value), datumDo, ref vypis))
                {
                    chori_success.Text = "v systeme nie su ziadne testy";
                    return;
                }
                var f = new WindowOut(vypis);
                f.Text = "Vypis vsetkych chorych v danom casovom obdobi";
                f.Show();
                chori_success.Text = "vypis je v novom okne";
                return;
            }
            if (chori_kraj.Value != 0 && chori_okres.Value == 0)
            {
                string vypis = "";
                if (!_app.O11_VypisChorychKraj((int)(chori_kraj.Value), (int)(chori_pocet_dni.Value), datumDo, ref vypis))
                {
                    chori_success.Text = "kraj so zadanym ID nema ziadne zaznamy";
                    return;
                }
                var f = new WindowOut(vypis);
                f.Text = "Vypis vsetkych chorych v danom kraji v danom casovom obdobi";
                f.Show();
                chori_success.Text = "vypis je v novom okne";
                return;
            }
            if (chori_okres.Value != 0 && chori_kraj.Value == 0)
            {
                string vypis = "";
                if (!_app.O10_VypisChorychOkres((int)(chori_okres.Value), (int)(chori_pocet_dni.Value), datumDo, ref vypis))
                {
                    chori_success.Text = "okres so zadanym ID nema ziadne zaznamy";
                    return;
                }
                var f = new WindowOut(vypis);
                f.Text = "Vypis vsetkych chorych v danom okrese v danom casovom obdobi";
                f.Show();
                chori_success.Text = "vypis je v novom okne";
                return;
            }
        }

        private void btn_o14_chori_kraj_Click(object sender, EventArgs e)
        {
            var datumDo = new DateTime(chori_datum.Value.Year, chori_datum.Value.Month, chori_datum.Value.Day);
            string vypis = "";
            if (!_app.O14_VypisKrajovPodlaChorych((int)(chori_pocet_dni.Value), datumDo, ref vypis))
            {
                chori_success.Text = "nastala chyba";
                return;
            }
            var f = new WindowOut(vypis);
            f.Text = "Vypis krajov podla poctu chorych v danom casovom obdobi";
            f.Show();
            chori_success.Text = "vypis je v novom okne";
            return;
        }

        private void btn_o13_chori_okres_Click(object sender, EventArgs e)
        {
            var datumDo = new DateTime(chori_datum.Value.Year, chori_datum.Value.Month, chori_datum.Value.Day);
            string vypis = "";
            if (!_app.O13_VypisOkresovPodlaChorych((int)(chori_pocet_dni.Value), datumDo, ref vypis))
            {
                chori_success.Text = "nastala chyba";
                return;
            }
            var f = new WindowOut(vypis);
            f.Text = "Vypis okresov podla poctu chorych v danom casovom obdobi";
            f.Show();
            chori_success.Text = "vypis je v novom okne";
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //vypis testov v kraji podla RC
            string vypis = "";
            if (!_app.NovaFunkcionalita((int)nove_kraj.Value, ref vypis))
            {
                return;
            }
            var f = new WindowOut(vypis);
            f.Text = "Vypis testov podla Rodneho Cisla";
            f.Show();
            return;
        }
    }
}
