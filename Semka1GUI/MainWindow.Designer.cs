
namespace Semka1GUI
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn17 = new System.Windows.Forms.Button();
            this.btn01 = new System.Windows.Forms.Button();
            this.osoba_meno = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.osoba_priezvisko = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.osoba_rod_cislo = new System.Windows.Forms.TextBox();
            this.osoba_datum_narodenia = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.osoba_success = new System.Windows.Forms.Label();
            this.pcr_success = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pcr_pracovisko = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.pcr_okres = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.pcr_kraj = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.pcr_rod_cislo = new System.Windows.Forms.TextBox();
            this.pcr_positive = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pcr_poznamka = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.gen_testy = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.gen_osoby = new System.Windows.Forms.NumericUpDown();
            this.btn_gen = new System.Windows.Forms.Button();
            this.gen_success = new System.Windows.Forms.Label();
            this.o02_success = new System.Windows.Forms.Label();
            this.btn02 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.o02_pcr_id = new System.Windows.Forms.NumericUpDown();
            this.o02_rod_cislo = new System.Windows.Forms.TextBox();
            this.btn_dummy = new System.Windows.Forms.Button();
            this.btn_vypis_vsetko = new System.Windows.Forms.Button();
            this.hladaj_iba_pozitivne = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.hladaj_prac = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.hladaj_okres = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.hladaj_kraj = new System.Windows.Forms.NumericUpDown();
            this.hladaj_success = new System.Windows.Forms.Label();
            this.btn_hladaj_testy_datum = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.hladaj_dat_od = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.hladaj_dat_do = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.pcr_pracovisko)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcr_okres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcr_kraj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gen_testy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gen_osoby)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.o02_pcr_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hladaj_prac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hladaj_okres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hladaj_kraj)).BeginInit();
            this.SuspendLayout();
            // 
            // btn17
            // 
            this.btn17.Location = new System.Drawing.Point(536, 26);
            this.btn17.Name = "btn17";
            this.btn17.Size = new System.Drawing.Size(75, 23);
            this.btn17.TabIndex = 3;
            this.btn17.Text = "Vloz Osobu";
            this.btn17.Click += new System.EventHandler(this.btn17_Click);
            // 
            // btn01
            // 
            this.btn01.Location = new System.Drawing.Point(442, 115);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(152, 23);
            this.btn01.TabIndex = 2;
            this.btn01.Text = "Vloz PCR test";
            this.btn01.UseVisualStyleBackColor = true;
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // osoba_meno
            // 
            this.osoba_meno.Location = new System.Drawing.Point(12, 26);
            this.osoba_meno.Name = "osoba_meno";
            this.osoba_meno.Size = new System.Drawing.Size(100, 23);
            this.osoba_meno.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Meno";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Priezvisko";
            // 
            // osoba_priezvisko
            // 
            this.osoba_priezvisko.Location = new System.Drawing.Point(118, 26);
            this.osoba_priezvisko.Name = "osoba_priezvisko";
            this.osoba_priezvisko.Size = new System.Drawing.Size(100, 23);
            this.osoba_priezvisko.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Rodne cislo";
            // 
            // osoba_rod_cislo
            // 
            this.osoba_rod_cislo.Location = new System.Drawing.Point(224, 26);
            this.osoba_rod_cislo.Name = "osoba_rod_cislo";
            this.osoba_rod_cislo.Size = new System.Drawing.Size(100, 23);
            this.osoba_rod_cislo.TabIndex = 8;
            // 
            // osoba_datum_narodenia
            // 
            this.osoba_datum_narodenia.Location = new System.Drawing.Point(330, 26);
            this.osoba_datum_narodenia.Name = "osoba_datum_narodenia";
            this.osoba_datum_narodenia.Size = new System.Drawing.Size(200, 23);
            this.osoba_datum_narodenia.TabIndex = 10;
            this.osoba_datum_narodenia.Value = new System.DateTime(2021, 10, 29, 0, 0, 0, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(330, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "Datum narodenia";
            // 
            // osoba_success
            // 
            this.osoba_success.AutoSize = true;
            this.osoba_success.Location = new System.Drawing.Point(617, 30);
            this.osoba_success.Name = "osoba_success";
            this.osoba_success.Size = new System.Drawing.Size(19, 15);
            this.osoba_success.TabIndex = 12;
            this.osoba_success.Text = "    ";
            // 
            // pcr_success
            // 
            this.pcr_success.AutoSize = true;
            this.pcr_success.Location = new System.Drawing.Point(600, 119);
            this.pcr_success.Name = "pcr_success";
            this.pcr_success.Size = new System.Drawing.Size(19, 15);
            this.pcr_success.TabIndex = 13;
            this.pcr_success.Text = "    ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(224, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "Pracovisko (ID)";
            // 
            // pcr_pracovisko
            // 
            this.pcr_pracovisko.Location = new System.Drawing.Point(224, 83);
            this.pcr_pracovisko.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.pcr_pracovisko.Name = "pcr_pracovisko";
            this.pcr_pracovisko.Size = new System.Drawing.Size(100, 23);
            this.pcr_pracovisko.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(118, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "Okres (ID)";
            // 
            // pcr_okres
            // 
            this.pcr_okres.Location = new System.Drawing.Point(118, 83);
            this.pcr_okres.Maximum = new decimal(new int[] {
            79,
            0,
            0,
            0});
            this.pcr_okres.Name = "pcr_okres";
            this.pcr_okres.Size = new System.Drawing.Size(100, 23);
            this.pcr_okres.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "Kraj (ID)";
            // 
            // pcr_kraj
            // 
            this.pcr_kraj.Location = new System.Drawing.Point(12, 83);
            this.pcr_kraj.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.pcr_kraj.Name = "pcr_kraj";
            this.pcr_kraj.Size = new System.Drawing.Size(100, 23);
            this.pcr_kraj.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(330, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 15);
            this.label9.TabIndex = 21;
            this.label9.Text = "Rod. cislo pacienta";
            // 
            // pcr_rod_cislo
            // 
            this.pcr_rod_cislo.Location = new System.Drawing.Point(330, 83);
            this.pcr_rod_cislo.Name = "pcr_rod_cislo";
            this.pcr_rod_cislo.Size = new System.Drawing.Size(106, 23);
            this.pcr_rod_cislo.TabIndex = 20;
            // 
            // pcr_positive
            // 
            this.pcr_positive.AutoSize = true;
            this.pcr_positive.Location = new System.Drawing.Point(442, 90);
            this.pcr_positive.Name = "pcr_positive";
            this.pcr_positive.Size = new System.Drawing.Size(152, 19);
            this.pcr_positive.TabIndex = 22;
            this.pcr_positive.Text = "Pozitivny vysledok testu";
            this.pcr_positive.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 15);
            this.label5.TabIndex = 25;
            this.label5.Text = "Poznamka";
            // 
            // pcr_poznamka
            // 
            this.pcr_poznamka.Location = new System.Drawing.Point(80, 116);
            this.pcr_poznamka.Name = "pcr_poznamka";
            this.pcr_poznamka.Size = new System.Drawing.Size(356, 23);
            this.pcr_poznamka.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(118, 156);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 15);
            this.label10.TabIndex = 29;
            this.label10.Text = "Pocet testov";
            // 
            // gen_testy
            // 
            this.gen_testy.Location = new System.Drawing.Point(118, 175);
            this.gen_testy.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.gen_testy.Name = "gen_testy";
            this.gen_testy.Size = new System.Drawing.Size(100, 23);
            this.gen_testy.TabIndex = 28;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 156);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 15);
            this.label11.TabIndex = 27;
            this.label11.Text = "Pocet osob";
            // 
            // gen_osoby
            // 
            this.gen_osoby.Location = new System.Drawing.Point(12, 175);
            this.gen_osoby.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.gen_osoby.Name = "gen_osoby";
            this.gen_osoby.Size = new System.Drawing.Size(100, 23);
            this.gen_osoby.TabIndex = 26;
            // 
            // btn_gen
            // 
            this.btn_gen.Location = new System.Drawing.Point(224, 175);
            this.btn_gen.Name = "btn_gen";
            this.btn_gen.Size = new System.Drawing.Size(111, 23);
            this.btn_gen.TabIndex = 30;
            this.btn_gen.Text = "Generuj data";
            this.btn_gen.Click += new System.EventHandler(this.btn_gen_Click);
            // 
            // gen_success
            // 
            this.gen_success.AutoSize = true;
            this.gen_success.Location = new System.Drawing.Point(341, 179);
            this.gen_success.Name = "gen_success";
            this.gen_success.Size = new System.Drawing.Size(19, 15);
            this.gen_success.TabIndex = 31;
            this.gen_success.Text = "    ";
            // 
            // o02_success
            // 
            this.o02_success.AutoSize = true;
            this.o02_success.Location = new System.Drawing.Point(341, 240);
            this.o02_success.Name = "o02_success";
            this.o02_success.Size = new System.Drawing.Size(19, 15);
            this.o02_success.TabIndex = 37;
            this.o02_success.Text = "    ";
            // 
            // btn02
            // 
            this.btn02.Location = new System.Drawing.Point(230, 236);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(105, 23);
            this.btn02.TabIndex = 36;
            this.btn02.Text = "Vyhladaj test";
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(118, 217);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(106, 15);
            this.label13.TabIndex = 35;
            this.label13.Text = "Rod. cislo pacienta";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 217);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(87, 15);
            this.label14.TabIndex = 33;
            this.label14.Text = "Cislo PCR testu";
            // 
            // o02_pcr_id
            // 
            this.o02_pcr_id.Location = new System.Drawing.Point(12, 236);
            this.o02_pcr_id.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.o02_pcr_id.Name = "o02_pcr_id";
            this.o02_pcr_id.Size = new System.Drawing.Size(100, 23);
            this.o02_pcr_id.TabIndex = 32;
            // 
            // o02_rod_cislo
            // 
            this.o02_rod_cislo.Location = new System.Drawing.Point(118, 237);
            this.o02_rod_cislo.Name = "o02_rod_cislo";
            this.o02_rod_cislo.Size = new System.Drawing.Size(106, 23);
            this.o02_rod_cislo.TabIndex = 38;
            // 
            // btn_dummy
            // 
            this.btn_dummy.Location = new System.Drawing.Point(12, 265);
            this.btn_dummy.Name = "btn_dummy";
            this.btn_dummy.Size = new System.Drawing.Size(75, 23);
            this.btn_dummy.TabIndex = 39;
            this.btn_dummy.Text = "Dummy";
            this.btn_dummy.UseVisualStyleBackColor = true;
            this.btn_dummy.Click += new System.EventHandler(this.btn_dummy_Click);
            // 
            // btn_vypis_vsetko
            // 
            this.btn_vypis_vsetko.Location = new System.Drawing.Point(93, 266);
            this.btn_vypis_vsetko.Name = "btn_vypis_vsetko";
            this.btn_vypis_vsetko.Size = new System.Drawing.Size(85, 23);
            this.btn_vypis_vsetko.TabIndex = 40;
            this.btn_vypis_vsetko.Text = "Vypis vsetko";
            this.btn_vypis_vsetko.UseVisualStyleBackColor = true;
            this.btn_vypis_vsetko.Click += new System.EventHandler(this.btn_vypis_vsetko_Click);
            // 
            // hladaj_iba_pozitivne
            // 
            this.hladaj_iba_pozitivne.AutoSize = true;
            this.hladaj_iba_pozitivne.Location = new System.Drawing.Point(12, 349);
            this.hladaj_iba_pozitivne.Name = "hladaj_iba_pozitivne";
            this.hladaj_iba_pozitivne.Size = new System.Drawing.Size(130, 19);
            this.hladaj_iba_pozitivne.TabIndex = 51;
            this.hladaj_iba_pozitivne.Text = "Hladaj iba pozitivne";
            this.hladaj_iba_pozitivne.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(224, 301);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(86, 15);
            this.label16.TabIndex = 48;
            this.label16.Text = "Pracovisko (ID)";
            // 
            // hladaj_prac
            // 
            this.hladaj_prac.Location = new System.Drawing.Point(224, 320);
            this.hladaj_prac.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.hladaj_prac.Name = "hladaj_prac";
            this.hladaj_prac.Size = new System.Drawing.Size(100, 23);
            this.hladaj_prac.TabIndex = 47;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(118, 301);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 15);
            this.label17.TabIndex = 46;
            this.label17.Text = "Okres (ID)";
            // 
            // hladaj_okres
            // 
            this.hladaj_okres.Location = new System.Drawing.Point(118, 320);
            this.hladaj_okres.Maximum = new decimal(new int[] {
            79,
            0,
            0,
            0});
            this.hladaj_okres.Name = "hladaj_okres";
            this.hladaj_okres.Size = new System.Drawing.Size(100, 23);
            this.hladaj_okres.TabIndex = 45;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 301);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(49, 15);
            this.label18.TabIndex = 44;
            this.label18.Text = "Kraj (ID)";
            // 
            // hladaj_kraj
            // 
            this.hladaj_kraj.Location = new System.Drawing.Point(12, 320);
            this.hladaj_kraj.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.hladaj_kraj.Name = "hladaj_kraj";
            this.hladaj_kraj.Size = new System.Drawing.Size(100, 23);
            this.hladaj_kraj.TabIndex = 43;
            // 
            // hladaj_success
            // 
            this.hladaj_success.AutoSize = true;
            this.hladaj_success.Location = new System.Drawing.Point(276, 353);
            this.hladaj_success.Name = "hladaj_success";
            this.hladaj_success.Size = new System.Drawing.Size(19, 15);
            this.hladaj_success.TabIndex = 42;
            this.hladaj_success.Text = "    ";
            // 
            // btn_hladaj_testy_datum
            // 
            this.btn_hladaj_testy_datum.Location = new System.Drawing.Point(148, 349);
            this.btn_hladaj_testy_datum.Name = "btn_hladaj_testy_datum";
            this.btn_hladaj_testy_datum.Size = new System.Drawing.Size(122, 23);
            this.btn_hladaj_testy_datum.TabIndex = 41;
            this.btn_hladaj_testy_datum.Text = "Vyhladaj PCR testy";
            this.btn_hladaj_testy_datum.UseVisualStyleBackColor = true;
            this.btn_hladaj_testy_datum.Click += new System.EventHandler(this.btn_hladaj_testy_datum_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(330, 301);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 15);
            this.label12.TabIndex = 53;
            this.label12.Text = "Datum od";
            // 
            // hladaj_dat_od
            // 
            this.hladaj_dat_od.Location = new System.Drawing.Point(330, 320);
            this.hladaj_dat_od.Name = "hladaj_dat_od";
            this.hladaj_dat_od.Size = new System.Drawing.Size(200, 23);
            this.hladaj_dat_od.TabIndex = 52;
            this.hladaj_dat_od.Value = new System.DateTime(2021, 9, 29, 0, 0, 0, 0);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(536, 301);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 15);
            this.label15.TabIndex = 55;
            this.label15.Text = "Datum do";
            // 
            // hladaj_dat_do
            // 
            this.hladaj_dat_do.Location = new System.Drawing.Point(536, 320);
            this.hladaj_dat_do.Name = "hladaj_dat_do";
            this.hladaj_dat_do.Size = new System.Drawing.Size(200, 23);
            this.hladaj_dat_do.TabIndex = 54;
            this.hladaj_dat_do.Value = new System.DateTime(2021, 11, 2, 23, 59, 59, 0);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 641);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.hladaj_dat_do);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.hladaj_dat_od);
            this.Controls.Add(this.hladaj_iba_pozitivne);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.hladaj_prac);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.hladaj_okres);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.hladaj_kraj);
            this.Controls.Add(this.hladaj_success);
            this.Controls.Add(this.btn_hladaj_testy_datum);
            this.Controls.Add(this.btn_vypis_vsetko);
            this.Controls.Add(this.btn_dummy);
            this.Controls.Add(this.o02_rod_cislo);
            this.Controls.Add(this.o02_success);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.o02_pcr_id);
            this.Controls.Add(this.gen_success);
            this.Controls.Add(this.btn_gen);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.gen_testy);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.gen_osoby);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pcr_poznamka);
            this.Controls.Add(this.pcr_positive);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pcr_rod_cislo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pcr_pracovisko);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pcr_okres);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pcr_kraj);
            this.Controls.Add(this.pcr_success);
            this.Controls.Add(this.osoba_success);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.osoba_datum_narodenia);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.osoba_rod_cislo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.osoba_priezvisko);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.osoba_meno);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.btn17);
            this.Name = "MainWindow";
            this.Text = "AUS2 Semestrálna práca 1 Serdel Tomas";
            ((System.ComponentModel.ISupportInitialize)(this.pcr_pracovisko)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcr_okres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcr_kraj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gen_testy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gen_osoby)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.o02_pcr_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hladaj_prac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hladaj_okres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hladaj_kraj)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn17;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.TextBox osoba_meno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox osoba_priezvisko;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox osoba_rod_cislo;
        private System.Windows.Forms.DateTimePicker osoba_datum_narodenia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label osoba_success;
        private System.Windows.Forms.Label pcr_success;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown pcr_pracovisko;
        private System.Windows.Forms.NumericUpDown pcr_kraj;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown pcr_okres;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox pcr_rod_cislo;
        private System.Windows.Forms.CheckBox pcr_positive;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox pcr_poznamka;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown gen_testy;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown gen_osoby;
        private System.Windows.Forms.Button btn_gen;
        private System.Windows.Forms.Label gen_success;
        private System.Windows.Forms.Label o02_success;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown o02_pcr_id;
        private System.Windows.Forms.TextBox o02_rod_cislo;
        private System.Windows.Forms.Button btn_dummy;
        private System.Windows.Forms.Button btn_vypis_vsetko;
        private System.Windows.Forms.CheckBox hladaj_iba_pozitivne;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown hladaj_prac;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown hladaj_okres;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown hladaj_kraj;
        private System.Windows.Forms.Label hladaj_success;
        private System.Windows.Forms.Button btn_hladaj_testy_datum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker hladaj_dat_od;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker hladaj_dat_do;
    }
}

