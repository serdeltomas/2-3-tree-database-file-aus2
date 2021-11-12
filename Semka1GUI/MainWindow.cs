using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semka1;

namespace Semka1GUI
{
    public partial class osoba_vymaz_success : Form
    {

        private App _app = new App();
        public osoba_vymaz_success()
        {
            InitializeComponent();
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
                    osoba_rod_cislo.Text, osoba_datum_narodenia.Value)) osoba_success.Text = "uspesne vlozena";
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
                if (_app.O01_VlozPCR((int)pcr_pracovisko.Value, (int)pcr_okres.Value, (int)pcr_kraj.Value, pcr_rod_cislo.Text, pcr_datum_testu.Value, pcr_positive.Checked,
                    pcr_poznamka.Text)) pcr_success.Text = "uspesne vlozene";
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
            var f = new WindowOut("haaha\ntext here");
            f.Text = "ugabuga";
            f.Show();
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
            if (hladaj_dat_od.Value < hladaj_dat_od.Value)
            {
                hladaj_success.Text = "datum do je mensi ako datum od";
                return;
            }
            if(hladaj_kraj.Value == 0 && hladaj_okres.Value == 0 && hladaj_prac.Value == 0)
            {
                if (hladaj_iba_pozitivne.Checked)
                {
                    string vypis = "";
                    if (!_app.O08_VypisVsetkychPozitivnychTestovDatum(hladaj_dat_od.Value, hladaj_dat_do.Value, ref vypis))
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
                    if (!_app.O09_VypisVsetkychTestovDatum(hladaj_dat_od.Value, hladaj_dat_do.Value, ref vypis))
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
                if (!_app.O04_VypisPozitivnychOkresDatum((int)hladaj_okres.Value, hladaj_dat_od.Value, hladaj_dat_do.Value, ref vypis))
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
                if (!_app.O05_VypisVsetkychOkresDatum((int)hladaj_okres.Value, hladaj_dat_od.Value, hladaj_dat_do.Value, ref vypis))
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
                if (!_app.O06_VypisPozitivnychKrajDatum((int)hladaj_kraj.Value, hladaj_dat_od.Value, hladaj_dat_do.Value, ref vypis))
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
                if (!_app.O07_VypisVsetkychKrajDatum((int)hladaj_kraj.Value, hladaj_dat_od.Value, hladaj_dat_do.Value, ref vypis))
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
                if (!_app.O15_VypisVsetkychPracDatum((int)hladaj_prac.Value, hladaj_dat_od.Value, hladaj_dat_do.Value, ref vypis))
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
            return;
        }
    }
}
