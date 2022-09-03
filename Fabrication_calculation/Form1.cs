using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Fabrication_calculation
{

    public partial class Form1 : Form
    {
        private class Works
        {
            public int Operation_Name = -1;
            public double Time_start = 0;
            public double Time_end = 0;

            public Works (int i, double start, double end)
            {
                Operation_Name = i;
                Time_start = start;
                Time_end = end;
            }

        }
        private class WorksRAB
        {
            public List<Works> W = new List<Works>();
            public double zValue = 0;

            public void Zupdate()
            {
                zValue = 0;
                foreach (Works WW in W)
                {
                    zValue += WW.Time_end - WW.Time_start;
                }
            }
        }

        List<List<double>> Tablelast = new List<List<double>>();
        List<List<double>> Tablelast2 = new List<List<double>>();
        List<TextBox> Textbox6layer2 = new List<TextBox>();

        List<WorksRAB> WORKSRABLOBAL = new List<WorksRAB>();



        public int defoult_select_type_add_operation = 1;

        List<String> Names_operations;
        List<double> operations_K;

        List<double> B_times;

        List<double> Mach_coun_r;
        List<int> Mach_coun_d;
        List<double> Mach_K;
        List<double> Mach_K_dop;
        List<List<int>> Works_dop_rab = new List<List<int>>();

        List<PictureBox> Pictures = new List<PictureBox>();
        List<Point> Sv_p = new List<Point>();
        int SIzeX = 40;
        int SIzeY = 40;

        double Per_brak = 10;
        double Norm_vip = 330000;

        double Norm_zap = 0;

        double F_normal = 275 * 60 * 8 * 60;
        double F_de = 0;

        double R_z = 0;

        int ko = 0;
        int kd = 0;
        double Norm_den = 0;
        double T_smen = 0;
        int rab = 0;
        int dop_rab = 0;
        double TimeDay = 0;



        List<List<double>> COunt_machiens_time = new List<List<double>>();
        List<List<double>> Zadel = new List<List<double>>();
        List<double> Zadeln = new List<double>();
        List<double> Zadelmax = new List<double>();
        List<List<double>> ZadelGraph = new List<List<double>>();

        List<double> Traz = new List<double>();
        List<String> Names_operations_ = new List<string>();


        List<TextBox> Textbox6layer = new List<TextBox>();
        List<TextBox> Textbox6layerK = new List<TextBox>();

        public Form1()
        {
            InitializeComponent();
            Names_operations = new List<string>();
            operations_K = new List<double>();

            B_times = new List<double>();
            B_times.Add(0.018); B_times.Add(0.025);

            Mach_coun_r = new List<double>();
            Mach_coun_d = new List<int>();
            Mach_K = new List<double>();
            Mach_K_dop = new List<double>();

            comboBox2.SelectedIndex = 1;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox1.SelectedIndex = 3;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.SelectedIndex = 1;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.SelectedIndex = 1;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox5.SelectedIndex = 0;
            comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;


            comboBox6.Items.Add(ChartColorPalette.Berry);
            comboBox6.Items.Add(ChartColorPalette.Bright);
            comboBox6.Items.Add(ChartColorPalette.BrightPastel);
            comboBox6.Items.Add(ChartColorPalette.Chocolate);
            comboBox6.Items.Add(ChartColorPalette.EarthTones);
            comboBox6.Items.Add(ChartColorPalette.Excel);
            comboBox6.Items.Add(ChartColorPalette.Fire);
            comboBox6.Items.Add(ChartColorPalette.Grayscale);
            comboBox6.Items.Add(ChartColorPalette.Light);
            comboBox6.Items.Add(ChartColorPalette.None);
            comboBox6.Items.Add(ChartColorPalette.Pastel);
            comboBox6.Items.Add(ChartColorPalette.SeaGreen);
            comboBox6.Items.Add(ChartColorPalette.SemiTransparent);
            comboBox6.SelectedIndex = 9;
            comboBox6.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox7.SelectedIndex = 0;
            comboBox7.DropDownStyle = ComboBoxStyle.DropDownList;

            InicializePoles();
            Update_N_z();
            Update_F_de();
        }


        void InicializePoles()
        {
            textBox1.Text = Norm_vip.ToString();
            textBox2.Text = Per_brak.ToString();

            double d = F_normal;
            switch (comboBox1.SelectedIndex)
            {
                case 1: { d /= 60; break; }
                case 2: { d /= 3600; break; }
                case 3: { d /= 3600 * 8; break; }
                default: break;
            }
            textBox4.Text = d.ToString();

            Update_ListBox();

            int Select = listBox2.SelectedIndex;
            listBox2.Items.Clear();
            foreach (double Val in B_times)
            {
                listBox2.Items.Add(Val.ToString());
            }
            listBox2.SelectedIndex = Select;

            Update_F_de();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region r1
        private void button2_Click(object sender, EventArgs e)
        {
            Form_add_operation F = new Form_add_operation(this, listBox1.Items.Count + 1, defoult_select_type_add_operation);
            F.Show();
        }

        public void Add(String Name, double K)
        {
            Names_operations.Add(Name);
            operations_K.Add(K);

            Update_ListBox();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_ListBox();
        }
        private void Update_ListBox()
        {
            int Selected = listBox1.SelectedIndex;
            listBox1.Items.Clear();
            TextBox_names.Clear();
            for (int i = 0; i < Names_operations.Count; i++)
            {
                double d = operations_K[i];
                switch (comboBox2.SelectedIndex)
                {
                    case 1: { d /= 60; break; }
                    case 2: { d /= 3600; break; }
                    default: break;
                }
                String str = d.ToString("0.0000");
                listBox1.Items.Add(str);
                TextBox_names.Text += Names_operations[i] + "\r\n";
            }
            listBox1.SelectedIndex = Selected;

            Update_machience_coun();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int k = listBox1.SelectedIndex;
                Names_operations.RemoveAt(k);
                operations_K.RemoveAt(k);
                listBox1.Items.RemoveAt(k);
            }
            catch { }

            Update_ListBox();
        }
        #endregion

        #region r2

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                B_times.RemoveAt(listBox2.SelectedIndex);
            }
            catch { }


            Update_F_de();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form_add_B_time F = new Form_add_B_time(this);
            F.Show();
        }
        public void add_B_time(double Value)
        {
            B_times.Add(Value);

            int Select = listBox2.SelectedIndex;
            listBox2.Items.Clear();
            foreach (double Val in B_times)
            {
                listBox2.Items.Add(Val.ToString());
            }
            listBox2.SelectedIndex = Select;

            Update_F_de();
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Norm_vip = int.Parse(textBox1.Text);
            }
            catch { }



            Update_N_z();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text.Replace('.', ',');
            try
            {
                NumberStyles style = NumberStyles.Float;
                IFormatProvider provider = CultureInfo.CreateSpecificCulture("fr-FR");
                Per_brak = Double.Parse(textBox2.Text, style, provider);
            }
            catch { }
            try
            {
                NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
               | NumberStyles.AllowParentheses;
                IFormatProvider provider = NumberFormatInfo.InvariantInfo;
                Per_brak = Double.Parse(textBox2.Text, style, provider);
            }
            catch { }
            Update_N_z();
        }

        private void Update_N_z()
        {


            Norm_zap = (Per_brak / 100 + 1) * Norm_vip;

            textBox3.Text = Norm_zap.ToString();

            Update_R_z();
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = textBox4.Text.Replace('.', ',');
            try
            {
                NumberStyles style = NumberStyles.Float;
                IFormatProvider provider = CultureInfo.CreateSpecificCulture("fr-FR");
                F_normal = Double.Parse(textBox4.Text, style, provider);
            }
            catch { }
            try
            {
                NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
               | NumberStyles.AllowParentheses;
                IFormatProvider provider = NumberFormatInfo.InvariantInfo;
                F_normal = Double.Parse(textBox4.Text, style, provider);
            }
            catch { }
            switch (comboBox1.SelectedIndex)
            {
                case 1: { F_normal *= 60; break; }
                case 2: { F_normal *= 3600; break; }
                case 3: { F_normal *= 3600 * 8; break; }
                default: break;
            }
            Update_F_de();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            double d = F_normal;
            switch (comboBox1.SelectedIndex)
            {
                case 1: { d /= 60; break; }
                case 2: { d /= 3600; break; }
                case 3: { d /= 3600 * 8; break; }
                default: break;
            }
            textBox4.Text = d.ToString();
        }


        private void Update_F_de()
        {

            F_de = F_normal;
            double d = 1;
            foreach (double val in B_times)
            {
                F_de *= 1 - val;
            }

            d = F_de;
            switch (comboBox3.SelectedIndex)
            {
                case 1: { d /= 60; break; }
                case 2: { d /= 3600; break; }
                case 3: { d /= 3600 * 8; break; }
                default: break;
            }
            textBox5.Text = d.ToString("0.0000");

            Update_R_z();
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            double d = F_de;
            switch (comboBox3.SelectedIndex)
            {
                case 1: { d /= 60; break; }
                case 2: { d /= 3600; break; }
                case 3: { d /= 3600 * 8; break; }
                default: break;
            }
            textBox5.Text = d.ToString("0.0000");
        }


        private void Update_R_z()
        {
            R_z = F_de / Norm_zap;

            double d = R_z;
            switch (comboBox4.SelectedIndex)
            {
                case 1: { d /= 60; break; }
                case 2: { d /= 3600; break; }
                default: break;
            }
            textBox6.Text = d.ToString("0.00000");

            Update_machience_coun();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            double d = R_z;
            switch (comboBox4.SelectedIndex)
            {
                case 1: { d /= 60; break; }
                case 2: { d /= 3600; break; }
                default: break;
            }
            textBox6.Text = d.ToString("0.00000");
        }

        #endregion

        #region r3
        private void Update_machience_coun()
        {
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            listBox7.Items.Clear();
            Mach_coun_r.Clear();
            Mach_coun_d.Clear();
            Mach_K.Clear();
            Mach_K_dop.Clear();

            for (int i = 0; i < Names_operations.Count; i++)
            {
                listBox3.Items.Add(Names_operations[i]);

                double r = operations_K[i] / R_z;
                Mach_coun_r.Add(r);

                listBox4.Items.Add(r.ToString("0.000"));

                int d = (int)Math.Ceiling(r);
                listBox5.Items.Add(d.ToString());
                Mach_coun_d.Add(d);

                double k = r / d;
                Mach_K.Add(k);
                listBox6.Items.Add(k.ToString("0.000"));

                double kd = 1 - d * (1 - k);
                Mach_K_dop.Add(kd);
                listBox7.Items.Add(kd.ToString("0.000"));
            }
            updateChart();
            updateChart2();

            table();

        }

        #endregion

        #region r4
        private void table()
        {

            Norm_den = Norm_zap / (F_normal / (60 * 8 * 60));

            T_smen = 8 * 60;
            foreach (double d in B_times)
            {
                T_smen *= 1 - d;
            }

            int Count = Names_operations.Count;

            listBox8.Items.Clear();
            listBox9.Items.Clear();
            listBox10.Items.Clear();
            listBox11.Items.Clear();
            listBox12.Items.Clear();
            listBox13.Items.Clear();
            listBox14.Items.Clear();

            ko = 0;
            kd = 0;
            TimeDay = 0;

            double kdoss = 0;

            for (int i = 0; i < Count; i++)
            {
                ko += Mach_coun_d[i] - 1;
                kd += 1;

                listBox12.Items.Add(Names_operations[i]);
                listBox11.Items.Add((operations_K[i] / 60).ToString("0.0000"));
                listBox10.Items.Add((1 / (operations_K[i] / 60)).ToString("0.0000"));
                listBox9.Items.Add((Norm_den * operations_K[i] / 60).ToString("0.0000"));
                listBox8.Items.Add(Mach_coun_d[i] - 1);
                listBox13.Items.Add(1);
                listBox14.Items.Add((Mach_K_dop[i]).ToString("0.0000"));

                TimeDay += Norm_den * operations_K[i] / 60;
                kdoss += Mach_K_dop[i];
            }

            int rab = (int)Math.Ceiling(TimeDay / T_smen);



            textBox11.Text = ko.ToString();
            textBox10.Text = kd.ToString();

            textBox7.Text = Norm_den.ToString();
            textBox8.Text = T_smen.ToString();
            textBox9.Text = rab.ToString();

            textBox13.Text = TimeDay.ToString("0.0000");
            textBox12.Text = kdoss.ToString("0.0000");



            //целые разбиения

            double OSt_id = kdoss;

            Works_dop_rab = new List<List<int>>();

            Traz = new List<double>();
            Traz.Add(0);
            List<double> LD = new List<double>(Mach_K_dop);
            List<int> LDi = new List<int>();
            for (int i = 0; i < LD.Count; i++)
                LDi.Add(i);



            dop_rab = rab - Mach_coun_d.Sum() + Mach_coun_d.Count();

            WORKSRABLOBAL = new List<WorksRAB>();
            for (int j = 0; j < dop_rab; j++)
            {
                WORKSRABLOBAL.Add(new WorksRAB());

                for (int i = 0; i < LD.Count; i++)
                {
                    WORKSRABLOBAL[j].W.Add(new Works(-1,0,0));
                }
            }

            for (int j = 0; j < dop_rab; j++)
            {


                for (int i = 0; i < LD.Count; i++)
                {
                    if (LD[i] + WORKSRABLOBAL[j].zValue <= 1)
                    {

                        WORKSRABLOBAL[j].W[LDi[i]].Operation_Name = LDi[i];
                        WORKSRABLOBAL[j].W[LDi[i]].Time_start = WORKSRABLOBAL[j].zValue;
                        WORKSRABLOBAL[j].W[LDi[i]].Time_end = WORKSRABLOBAL[j].zValue + LD[i];
                        WORKSRABLOBAL[j].zValue += LD[i];

                        OSt_id -= LD[i];
                        LD.RemoveAt(i);
                        LDi.RemoveAt(i);
                        i--;
                    }

                }
            }
            if (OSt_id == 0)
            {
                
            }
            else
            {
              


            }



            updateENdform();


            updateChparam();
        }
        #endregion

        #region r5
        private void updateChart()
        {
            int L = 2;
            for (int i = 0; i < Mach_coun_d.Count; i++)
            {
                if (Mach_coun_d[i] + 1 > L) L = Mach_coun_d[i] + 1;
            }

            chart1.Series.Clear();
            if (comboBox6.SelectedIndex > -1)
                chart1.Palette = (ChartColorPalette)comboBox6.Items[comboBox6.SelectedIndex];
            double x = 0;
            //chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas["ChartArea1"].Axes[1].Enabled = AxisEnabled.False;

            for (int i = 0; i < Mach_coun_d.Count; i++)
            {


                if (chart1.Series.Count == i)
                {
                    chart1.Series.Add(Names_operations[i]);
                    chart1.Series[i].ChartType = SeriesChartType.Line;

                    switch (comboBox5.SelectedIndex)
                    {
                        case 0: { chart1.Series[Names_operations[i]].BorderWidth = 2; break; }
                        case 1: { chart1.Series[Names_operations[i]].BorderWidth = 3; break; }
                        default: break;
                    }
                }

                for (int j = 0; j < Mach_coun_d[i]; j++)
                {
                    switch (comboBox5.SelectedIndex)
                    {
                        case 0: { x = Mach_coun_d.Count - (double)i + (double)(j) / (double)L - 0.5; break; }
                        case 1: { x--; break; }
                        default: break;
                    }
                    double y = Mach_K[i];
                    chart1.Series[i].Points.AddXY(0, x);
                    chart1.Series[i].Points.AddXY(y, x);
                    chart1.Series[i].Points.AddXY(0, x);
                }
                switch (comboBox5.SelectedIndex)
                {
                    case 0:
                        {
                            chart1.Series[i].Points.AddXY(Mach_K[i], x);
                            chart1.Series[i].Points.AddXY(Mach_K[i], -999);
                            break;
                        }
                    case 1:
                        {
                            chart1.Series[i].Points.AddXY(Mach_K[i], x);
                            chart1.Series[i].Points.AddXY(Mach_K[i], x + Mach_coun_d[i] - 1);
                            chart1.Series[i].Points.AddXY(Mach_K[i], -999);
                            break;
                        }
                    default: break;
                }

            }



            switch (comboBox5.SelectedIndex)
            {
                case 0: { chart1.ChartAreas["ChartArea1"].Axes[1].Minimum = -1; chart1.ChartAreas["ChartArea1"].Axes[1].Maximum = Mach_coun_d.Count + 1; break; }
                case 1: { chart1.ChartAreas["ChartArea1"].Axes[1].Minimum = x - 2; chart1.ChartAreas["ChartArea1"].Axes[1].Maximum = 1; break; }
                default: break;
            }
            chart1.ChartAreas["ChartArea1"].Axes[0].Minimum = 0;
            chart1.ChartAreas["ChartArea1"].Axes[0].Maximum = 1;

            chart1.ChartAreas["ChartArea1"].Axes[0].Interval = 1;

        }

        private void updateChart2()
        {
            int L = 2;
            for (int i = 0; i < Mach_coun_d.Count; i++)
            {
                if (Mach_coun_d[i] + 1 > L) L = Mach_coun_d[i] + 1;
            }

            chart2.Series.Clear();
            if (comboBox6.SelectedIndex > -1)
                chart2.Palette = (ChartColorPalette)comboBox6.Items[comboBox6.SelectedIndex];
            double x = 0;
            chart2.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            chart2.ChartAreas["ChartArea1"].Axes[1].Enabled = AxisEnabled.False;

            for (int i = 0; i < Mach_coun_d.Count; i++)
            {
                if (chart2.Series.Count == i)
                {
                    chart2.Series.Add(Names_operations[i]);
                    chart2.Series[i].ChartType = SeriesChartType.Line;

                    switch (comboBox5.SelectedIndex)
                    {
                        case 0: { chart2.Series[Names_operations[i]].BorderWidth = 2; break; }
                        case 1: { chart2.Series[Names_operations[i]].BorderWidth = 3; break; }
                        default: break;
                    }
                }

                for (int j = 0; j < Mach_coun_d[i] - 1; j++)
                {
                    switch (comboBox5.SelectedIndex)
                    {
                        case 0: { x = Mach_coun_d.Count - (double)i - (double)(j) / (double)L - 0.5; break; }
                        case 1: { x--; break; }
                        default: break;
                    }
                    chart2.Series[i].Points.AddXY(0, x);
                    chart2.Series[i].Points.AddXY(1, x);
                    chart2.Series[i].Points.AddXY(0, x);
                }
                switch (comboBox5.SelectedIndex)
                {
                    case 0: { x = Mach_coun_d.Count - (double)i - ((double)Mach_coun_d[i] - 1) / L - 0.5; break; }
                    case 1: { x--; break; }
                    default: break;
                }
                double y = Mach_K_dop[i];
                chart2.Series[i].Points.AddXY(0, x);
                chart2.Series[i].Points.AddXY(y, x);
                chart2.Series[i].Points.AddXY(0, x);

                switch (comboBox5.SelectedIndex)
                {
                    case 0:
                        {
                            chart2.Series[i].Points.AddXY(Mach_K_dop[i], x);
                            chart2.Series[i].Points.AddXY(Mach_K_dop[i], -999);
                            break;
                        }
                    case 1:
                        {
                            chart2.Series[i].Points.AddXY(Mach_K_dop[i], x);
                            chart2.Series[i].Points.AddXY(Mach_K_dop[i], x);
                            chart2.Series[i].Points.AddXY(Mach_K_dop[i], -999);
                            break;
                        }
                    default: break;
                }
            }

            switch (comboBox5.SelectedIndex)
            {
                case 0: { chart2.ChartAreas["ChartArea1"].Axes[1].Minimum = -1; chart2.ChartAreas["ChartArea1"].Axes[1].Maximum = Mach_coun_d.Count; break; }
                case 1: { chart2.ChartAreas["ChartArea1"].Axes[1].Minimum = x - 2; chart2.ChartAreas["ChartArea1"].Axes[1].Maximum = 1; break; }
                default: break;
            }
            chart2.ChartAreas["ChartArea1"].Axes[0].Minimum = 0;
            chart2.ChartAreas["ChartArea1"].Axes[0].Maximum = 1;

            chart2.ChartAreas["ChartArea1"].Axes[0].Interval = 1;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateChart();
            updateChart2();
        }

        private void comboBox6_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            updateChart();
            updateChart2();
        }
        #endregion

        #region r6

        bool autoUpdate6 = false;

        void updateChparam()
        {
            autoUpdate6 = false;

            foreach (TextBox tb in Textbox6layer)
                tb.Dispose();
                foreach (TextBox tb in Textbox6layerK)
                tb.Dispose();
            Textbox6layerK.Clear();
            Textbox6layer.Clear();

            textBox14.Clear();
            for (int j = 0; j < Names_operations.Count; j++)
            {
                textBox14.Text += Names_operations[j];
                if (j != Names_operations.Count - 1) textBox14.Text += Environment.NewLine;
            }

                for (int j = 0; j < dop_rab; j++)
            {
                TextBox Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[5];
                Tb.Location = new Point(160 * j + 170, 30);
                Tb.AutoSize = false;
                Tb.Size = new Size(161, 36);
                Tb.Multiline = true;
                Tb.Enabled = false;
                Tb.Text = "Работник " + (j + 1).ToString();
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = j;
                Textbox6layer.Add(Tb);

                Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[5];
                Tb.Location = new Point(160 * j + 170, 65);
                Tb.AutoSize = false;
                Tb.Size = new Size(81, 36);
                Tb.Multiline = true;
                Tb.Enabled = false;
                Tb.Text = "К. начала работы";
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = j;
                Textbox6layer.Add(Tb);

                Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[5];
                Tb.Location = new Point(160 * j + 250, 65);
                Tb.AutoSize = false;
                Tb.Size = new Size(81, 36);
                Tb.Multiline = true;
                Tb.Enabled = false;
                Tb.Text = "К. окончание работы";
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = j;
                Textbox6layer.Add(Tb);

                Font F = new Font(Tb.Font.FontFamily, 10);

                Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[5];
                Tb.Location = new Point(160 * j + 170, 100);
                Tb.AutoSize = false;
                Tb.Size = new Size(81, 195);
                Tb.Multiline = true;
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = j;
                Tb.TextChanged += TB_start_TextChanged;
                for (int i = 0; i < Names_operations.Count; i++)
                {
                    foreach ( Works ww in WORKSRABLOBAL[j].W)
                    {
                        if (ww.Operation_Name == i)                   
                            switch(comboBox7.SelectedIndex)
                            {
                                case 0: { Tb.Text += (ww.Time_start*T_smen).ToString("0.0000"); break; }
                                case 1: { Tb.Text += ww.Time_start.ToString("0.0000"); break; }
                            }
                    }
                    if (i != Names_operations.Count - 1) Tb.Text += Environment.NewLine;
                }
                Tb.Font = F;
                Textbox6layer.Add(Tb);

                Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[5];
                Tb.Location = new Point(160 * j + 250, 100);
                Tb.AutoSize = false;
                Tb.Size = new Size(81, 195);
                Tb.Multiline = true;
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = j;
                Tb.TextChanged += TB_end_TextChanged;
                for (int i = 0; i < Names_operations.Count; i++)
                {
                    foreach (Works ww in WORKSRABLOBAL[j].W)
                    {
                        if (ww.Operation_Name == i)
                        {
                            switch (comboBox7.SelectedIndex)
                            {
                                case 0: { Tb.Text += (ww.Time_end * T_smen).ToString("0.0000"); break; }
                                case 1: { Tb.Text += ww.Time_end.ToString("0.0000"); break; }
                            }
                        }
                    }
                    if (i != Names_operations.Count - 1) Tb.Text += Environment.NewLine;
                }
                Tb.Font = F;
                Textbox6layer.Add(Tb);


                Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[5];
                Tb.Location = new Point(160 * j + 170, 295);
                Tb.AutoSize = false;
                Tb.Size = new Size(161, 20);
                Tb.Multiline = true;
                Tb.Enabled = false;
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = j;
                Textbox6layer.Add(Tb);
                Textbox6layerK.Add(Tb);
            }
            UpdateKostOperation();

            autoUpdate6 = true;
        }



        private void TB_start_TextChanged(object sender, EventArgs e)
        {
            if (autoUpdate6)
            {
                int I = (int)((sender as TextBox).Tag);

                List<string> K = (sender as TextBox).Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList<string>();
                for (int i = 0; i < K.Count; i++)
                {
                    if (i < Names_operations.Count)
                    {
                        if (K[i] != "")
                        {
                            string str = K[i];
                            double d = 0;
                            try { d = Double.Parse(str.Replace('.', ',')); } catch { }
                            switch (comboBox7.SelectedIndex)
                            {
                                case 0: { d /= T_smen; break; }
                                case 1: { break; }
                            }

                            WORKSRABLOBAL[I].W[i].Time_start = d;


                            if (WORKSRABLOBAL[I].W[i].Time_start < WORKSRABLOBAL[I].W[i].Time_end)
                            {
                                WORKSRABLOBAL[I].W[i].Operation_Name = i;
                            }
                            else
                            {
                                WORKSRABLOBAL[I].W[i].Operation_Name = -1;
                            }
                        }
                        else
                        {

                            WORKSRABLOBAL[I].W[i].Time_start = 0;
                            WORKSRABLOBAL[I].W[i].Operation_Name = -1;

                        }
                    }
                }
                updateENdform();
                UpdateKostOperation();
            }

        }

        private void TB_end_TextChanged(object sender, EventArgs e)
        {
            if (autoUpdate6)
            {
                int I = (int)((sender as TextBox).Tag);

                List<string> K = (sender as TextBox).Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList<string>();
                for (int i = 0; i < K.Count; i++)
                {
                    if (i < Names_operations.Count)
                    {
                        if (K[i] != "")
                        {
                            string str = K[i];
                            double d = 0;
                            try { d = Double.Parse(str.Replace('.', ',')); } catch { }
                            switch (comboBox7.SelectedIndex)
                            {
                                case 0: { d /= T_smen; break; }
                                case 1: { break; }
                            }

                            WORKSRABLOBAL[I].W[i].Time_end = d;

                            if (WORKSRABLOBAL[I].W[i].Time_start < WORKSRABLOBAL[I].W[i].Time_end)
                            {
                                WORKSRABLOBAL[I].W[i].Operation_Name = i ;
                            }
                            else
                            {
                                WORKSRABLOBAL[I].W[i].Operation_Name = -1;
                            }
                        }
                        else
                        {

                            WORKSRABLOBAL[I].W[i].Time_end = 0;
                            WORKSRABLOBAL[I].W[i].Operation_Name = -1;

                        }
                    }
                }
                UpdateKostOperation();
                updateENdform();
            }
        }


        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateChparam();
        }

        private void UpdateKostOperation()
        { 
            string Warning1_str = "Расределите всю работу  ";
            string Warning2_str = "Ошибка в интервале времени:  ";
            string Error1_str = "Пересечение по операциям  ";
            string Error2_str = "Пересечение работников  ";
            string Error3_str = "Работник перегружен";

            bool Warning1 = false;
            bool Warning2 = false;
            bool Error1 = false;
            bool Error2 = false;
            bool Error3 = false;

            for (int i = 0; i < WORKSRABLOBAL.Count; i++)
            {
                for (int j = 0; j < WORKSRABLOBAL[i].W.Count; j++)
                {
                    if ((WORKSRABLOBAL[i].W[j].Time_end > 1) || (WORKSRABLOBAL[i].W[j].Time_start < 0) || (WORKSRABLOBAL[i].W[j].Time_start > WORKSRABLOBAL[i].W[j].Time_end))
                    {
                        Warning2_str += "(" + (i + 1).ToString() + " раб. -" + (j + 1).ToString() + " опер. )"; 
                        Warning2 = true;
                    }
                    if (WORKSRABLOBAL[i].W[j].Time_start < 0) WORKSRABLOBAL[i].W[j].Time_start = 0;
                    if (WORKSRABLOBAL[i].W[j].Time_end > 1) WORKSRABLOBAL[i].W[j].Time_end = 1;
                }
            }

            for (int i = 0; i < WORKSRABLOBAL.Count; i++)
            {
                WORKSRABLOBAL[i].Zupdate();
               if (i < Textbox6layerK.Count)
                switch (comboBox7.SelectedIndex)
                {
                    case 0: { Textbox6layerK[i].Text = (WORKSRABLOBAL[i].zValue*T_smen).ToString("0.00"); break; }
                    case 1: { Textbox6layerK[i].Text = WORKSRABLOBAL[i].zValue.ToString("0.0000"); break; }
                }
                if(WORKSRABLOBAL[i].zValue > 1)
                {
                    Error3 = true;
                    Error3_str += "(" + (i+1).ToString() + "раб.)";
                }
            }
                

            List<double> LD = new List<double>(Mach_K_dop);

            for (int i = 0; i < WORKSRABLOBAL.Count; i++)
            {
                for (int j = 0; j < WORKSRABLOBAL[i].W.Count; j++)
                {
                    if (WORKSRABLOBAL[i].W[j].Operation_Name > -1)
                        LD[WORKSRABLOBAL[i].W[j].Operation_Name] -= WORKSRABLOBAL[i].W[j].Time_end - WORKSRABLOBAL[i].W[j].Time_start;
                }
            }

            textBox16.Clear();
            for (int i = 0; i < LD.Count; i++)
            {
                switch (comboBox7.SelectedIndex)
                {
                    case 0: { textBox16.Text += (LD[i]*T_smen).ToString("0.00"); break; }
                    case 1: { textBox16.Text += LD[i].ToString("0.0000"); break; }
                }
                
                if (i != LD.Count - 1) textBox16.Text += Environment.NewLine;
                if (Math.Abs(LD[i]) >= 0.001) Warning1 = true;
            }

            for (int i = 0; i < WORKSRABLOBAL.Count; i++)
            {
                for (int j = 0; j < WORKSRABLOBAL[i].W.Count; j++)
                {

                    for (int i2 = 0; i2 < WORKSRABLOBAL.Count; i2++)
                    {
                        for (int j2 = 0; j2 < WORKSRABLOBAL[i].W.Count; j2++)
                        {
                            if (i2 != i)
                            {

                                if ((WORKSRABLOBAL[i].W[j].Operation_Name == WORKSRABLOBAL[i2].W[j2].Operation_Name) && (WORKSRABLOBAL[i].W[j].Operation_Name != -1))
                                {
                                    double Ts1 = WORKSRABLOBAL[i].W[j].Time_start;
                                    double Te1 = WORKSRABLOBAL[i].W[j].Time_end;

                                    double Ts2 = WORKSRABLOBAL[i2].W[j2].Time_start;
                                    double Te2 = WORKSRABLOBAL[i2].W[j2].Time_end;

                                    if ((Ts1 > Ts2 && Ts1 < Te2) || (Te1 > Ts2 && Te1 < Te2))
                                    {
                                        Error2 = true;
                                        Error2_str += "(" + (i + 1).ToString() + " раб. X " + (i2 + 1).ToString() + " раб.) ";
                                    }
                                }
                            }
                        }
                    }
                }
            }


            for (int i = 0; i < WORKSRABLOBAL.Count; i++)
            {
                for (int j = 0; j < WORKSRABLOBAL[i].W.Count; j++)
                {
                    for (int j2 = 0; j2 < WORKSRABLOBAL[i].W.Count; j2++)
                    {
                        if (j != j2)
                        {
                            double Ts1 = WORKSRABLOBAL[i].W[j].Time_start;
                            double Te1 = WORKSRABLOBAL[i].W[j].Time_end;

                            double Ts2 = WORKSRABLOBAL[i].W[j2].Time_start;
                            double Te2 = WORKSRABLOBAL[i].W[j2].Time_end;

                            if ((Ts1 - Ts2 > 0.005 && Ts1 - Te2 < -0.005) || (Te1 - Ts2 > 0.005 && Te1 - Te2 < -0.005))
                            {
                                Error1 = true;
                                Error1_str += "(раб. " +(i+1).ToString()+"(" + (j + 1).ToString() + " опер. c " + (j2 + 1).ToString() + " опер.)) ";
                            }
                        }
                    }
                }
            }


            label20.Visible = Warning1;
            label21.Visible = Error1;
            label22.Visible = Error2;
            label23.Visible = Warning2;
            label24.Visible = Error3;

            label20.Text = Warning1_str;
            label21.Text = Error1_str;
            label22.Text = Error2_str;
            label23.Text = Warning2_str;
            label24.Text = Error3_str;
        }

            #endregion

         void updateENdform()
        {
            Traz.Clear();
            for (int i = 0; i < WORKSRABLOBAL.Count; i++)
            {
                for (int j = 0; j < WORKSRABLOBAL[i].W.Count; j++)
                {
                   Traz.Add(WORKSRABLOBAL[i].W[j].Time_end);
                   Traz.Add(WORKSRABLOBAL[i].W[j].Time_start);
                }
            }

            Traz.Sort();
            for (int i = 0; i < Traz.Count; i++)
            {
                for (int j = 0; j < Traz.Count; j++)
                {
                    if (i != j)
                    {
                        if (Traz[i] == Traz[j])
                        {
                            Traz.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }

            COunt_machiens_time = new List<List<double>>();

            List<double> operations_K_ = new List<double>();
            operations_K_.Add(R_z);
            operations_K_.AddRange(operations_K);
            operations_K_.Add(R_z);
            List<int> Mach_coun_d_ = new List<int>();
            Mach_coun_d_.Add(2);
            Mach_coun_d_.AddRange(Mach_coun_d);
            Mach_coun_d_.Add(2);

            Names_operations_ = new List<string>();
            Names_operations_.Add("0");
            Names_operations_.AddRange(Names_operations);
            Names_operations_.Add((Names_operations.Count + 1).ToString());

            Traz.Add(1);

            for (int i = 0; i < Mach_coun_d_.Count; i++)
            {
                COunt_machiens_time.Add(new List<double>());

                for (int j = 0; j < Traz.Count - 1; j++)
                {
                    COunt_machiens_time[i].Add(Mach_coun_d_[i] - 1);


                    for (int k = 0; k < WORKSRABLOBAL.Count; k++)
                    {

                        for (int k2 = 0; k2 < WORKSRABLOBAL[k].W.Count; k2++)
                        {
                            double T1 = WORKSRABLOBAL[k].W[k2].Time_start;
                            double T2 = WORKSRABLOBAL[k].W[k2].Time_end;

                            if (WORKSRABLOBAL[k].W[k2].Operation_Name + 1 == i)
                                if (T1 <= Traz[j] && T2 >= Traz[j + 1])
                                {
                                    COunt_machiens_time[i][j] += 1;
                                }
                        }
                    }
                }
            }


            Zadel = new List<List<double>>();
            Zadeln = new List<double>();
            Zadelmax = new List<double>();
            ZadelGraph = new List<List<double>>();

            for (int i = 0; i < COunt_machiens_time.Count-1; i++)
            {
                Zadel.Add(new List<double>());

                double minZ = 0;
                double z2 = 0;
                double maxZ = 0;

                for (int j = 0; j < COunt_machiens_time[i].Count; j++)
                {

                    double TimeLocal = (Traz[j + 1] - Traz[j]) * 460 * 60;

                    double kj = (COunt_machiens_time[i][j] / operations_K_[i] - COunt_machiens_time[i + 1][j] / operations_K_[i + 1]);
                    double z = TimeLocal * kj;
                    z2 += z;

                    minZ = Math.Min(minZ, z2);
                    maxZ = Math.Max(maxZ, z2);

                    Zadel[i].Add(z);
                }
                Zadelmax.Add(maxZ + Math.Max(0, -minZ));
                Zadeln.Add(Math.Max(0, -minZ));

            }
            for (int i = 0; i < Zadel.Count; i++)
            {
                ZadelGraph.Add(new List<double>());
                double z = Zadeln[i];
                ZadelGraph[i].Add(Zadeln[i]);

                for (int j = 0; j < Zadel[i].Count; j++)
                {
                    z += Zadel[i][j];
                    ZadelGraph[i].Add(z);
                }
                //ZadelGraph[i].Add(Zadeln[i]);
            }

            UpdateTabl();
            Update_picture4();
            Update_picture3();
        }


        void UpdateTabl()
        {
            Tablelast = new List<List<double>>();
            Tablelast2 = new List<List<double>>();

            for (int i = 0; i < Zadel.Count; i++)
            {
                Tablelast.Add(new List<double>());
                Tablelast2.Add(new List<double>());

                double k = -9999999;
                double Tgl = 0;
                double Zgl = 0;

                for (int j = 0; j < Zadel[i].Count; j++)
                {
                    double T = Traz[j + 1] - Traz[j];
                    double Z = Zadel[i][j];
                    double Kn = Zadel[i][j] / T;

                    if (Math.Abs(k - Kn) > 0.001)
                    {
                        if (j != 0)
                        {
                            Tablelast[i].Add(Zgl);
                            Tablelast2[i].Add(Tgl* T_smen);
                        }

                        k = Zadel[i][j] / T;
                        Zgl = Z;
                        Tgl = T;

                        if (j == Zadel[i].Count - 1)
                        {
                            Tablelast[i].Add(Zgl);
                            Tablelast2[i].Add(Tgl* T_smen);
                        }
                    }
                    else
                    {
                        Zgl += Z;
                        Tgl += T;

                        if (j == Zadel[i].Count -1)
                        {
                            Tablelast[i].Add(Zgl);
                            Tablelast2[i].Add(Tgl* T_smen);
                        }
                    }

                }       


            }


            foreach (TextBox tb in Textbox6layer2)
                tb.Dispose();
            Textbox6layer2.Clear();

            for (int i = 0; i < Tablelast.Count; i++)
            {

                TextBox Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[7];
                Tb.Location = new Point(80 *i + 60, 30);
                Tb.AutoSize = false;
                Tb.Size = new Size(80, 36);
                Tb.Multiline = true;
                Tb.Enabled = false;
                Tb.Text = "T " + i.ToString() + "-" + (i + 1).ToString() + Environment.NewLine + "(мин)";
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = i;
                Textbox6layer2.Add(Tb);

                Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[7];
                Tb.Location = new Point(80 * i + 60, 200);
                Tb.AutoSize = false;
                Tb.Size = new Size(80, 36);
                Tb.Multiline = true;
                Tb.Enabled = false;
                Tb.Text = "Z " + i.ToString() + "-" + (i + 1).ToString() + Environment.NewLine +"(шт)";
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = i;
                Textbox6layer2.Add(Tb);

                Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[7];
                Tb.Location = new Point(80 * i + 60, 65);
                Tb.AutoSize = false;
                Tb.Size = new Size(80, 100);
                Tb.Multiline = true;
                Tb.ReadOnly = true;
                Tb.Text = "";
                for (int j = 0; j < Tablelast2[i].Count; j++)
                {
                    Tb.Text += Tablelast2[i][j].ToString("0.000") + Environment.NewLine;
                }
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = i;
                Textbox6layer2.Add(Tb);

                Tb = new TextBox();
                Tb.Parent = tabControl1.TabPages[7];
                Tb.Location = new Point(80 * i + 60, 235);
                Tb.AutoSize = false;
                Tb.Size = new Size(80, 100);
                Tb.Multiline = true;
                Tb.ReadOnly = true;
                Tb.Text = "";
                for (int j = 0; j < Tablelast[i].Count; j++)
                {
                    Tb.Text += Tablelast[i][j].ToString("0.000") + Environment.NewLine;
                }
                Tb.TextAlign = HorizontalAlignment.Center;
                Tb.Tag = i;
                Textbox6layer2.Add(Tb);
            }
        }

        void Update_picture3()
        {
            const int Xm = 600;
            const int Ym = 40;

            const int Xn = 80;
            const int Yn = 50;

            int xo = (int)((COunt_machiens_time.Count+3) * Ym);

            Bitmap b = new Bitmap(1200, xo);
            pictureBox12.Image = b;
            Graphics g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen P1 = new Pen(Color.Black);
            P1.Width = 2;
            Font drawFont = new Font("Arial", 14);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;



            int L = 2;
            for (int i = 0; i < Mach_coun_d.Count; i++)
            {
                if (Mach_coun_d[i] + 1 > L) L = Mach_coun_d[i] + 1;
            }

            g.DrawLine(P1, new Point((int)(Xn), (int)(Yn)), new Point((int)(Xn), (int)(Ym * (Mach_coun_d.Count+2) + Yn)));

            for (int i = 0; i < COunt_machiens_time.Count; i++)
            {
                double x = 0;

                g.DrawString(Names_operations_[i], drawFont, drawBrush, Xn, (int)((((double)i +0.5) * Ym) + Yn - 7), drawFormat);



                for (int j = 0; j < Traz.Count - 1; j++)
                {



                    for (int k = 0; k < COunt_machiens_time[i][j]; k++)
                    {
                        x = (double)i - (double)(k) / (double)L + 0.5;
                        Point A = new Point((int)(Traz[j] * Xm) + Xn, (int)(x * Ym) + Yn);
                        Point B = new Point((int)(Traz[j + 1] * Xm) + Xn, (int)(x * Ym) + Yn);
                        g.DrawLine(P1, A, B);
                    }

                }

            }
        


        }
        void Update_picture4()
        {

            const int Xm = 600;
            const int Ym = 200;
            const int Ym2 = 40;

            const int Xn = 80;
            const int Yn = 50;
            int xo = (int)((ZadelGraph.Count + 3) * Ym);

            Bitmap b = new Bitmap(1200, xo);
            pictureBox13.Image = b;

            Graphics g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen P1 = new Pen(Color.Gray);
            Pen P2 = new Pen(Color.FromArgb(0, 0, 50));
            P2.Width = 2;
            Brush B1 = new SolidBrush(Color.FromArgb(200, 200, 230));
            Brush B2 = new SolidBrush(Color.FromArgb(240, 240, 255));

            P1.Width = 1;
            Font drawFont = new Font("Arial", 14);
            Font drawFont2 = new Font("Arial", 8);
            SolidBrush drawBrush = new SolidBrush( Color.Black);
            SolidBrush drawBrush2 = new SolidBrush(Color.Gray);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            StringFormat drawFormat2 = new StringFormat();
            drawFormat2.FormatFlags = StringFormatFlags.NoClip;
            double x = 0;





            g.DrawLine(P1, new Point((int)(Xn), (int)(Yn)), new Point((int)(Xn), (int)(Ym - Yn)) );
            g.DrawLine(P1, new Point((int)(Xm + Xn), (int)(Yn)), new Point((int)(Xm + Xn), (int)(Ym - Yn)));
            for (int i = 0; i < ZadelGraph.Count; i++)
            {
                x = i + 1;

                g.DrawString(Names_operations_[i], drawFont, drawBrush, Xn/2, (int)((((double)i ) * Ym) + Yn*2 ), drawFormat);



                g.FillPolygon(B2, new Point[] { new Point((int)(1 * Xm) + Xn, (int)(x * Ym) + Yn), new Point((int)(0 * Xm) + Xn, (int)(x * Ym) + Yn), new Point((int)(0 * Xm) + Xn, (int)(x * Ym) - Yn), new Point((int)(1 * Xm) + Xn, (int)(x * Ym) - Yn), });



                //g.DrawLine(P1, new Point((int)(1 * Xm) + Xn, (int)(x * Ym) + Yn), new Point((int)(0 * Xm) + Xn, (int)(x * Ym) + Yn));

                //g.DrawLine(P1, new Point((int)(1 * Xm) + Xn, (int)(x * Ym) - Yn), new Point((int)(0 * Xm) + Xn, (int)(x * Ym) - Yn));

                List<Point>P = new List<Point>();
                List<Point> PF = new List<Point>();

                P.Add(new Point((int)(0 * Xm) + Xn, (int)(x * Ym) + Yn));
                for (int j = 0; j < ZadelGraph[i].Count; j++)
                {
                    if (j < Traz.Count)
                    {


                        double y = Traz[j];
                        double xx = x - ZadelGraph[i][j] / Zadelmax[i] / 2;
                        if (Zadelmax[i] == 0) xx = x - 0.5;
                        Point A = new Point((int)(y * Xm) + Xn, (int)(xx * Ym) + Yn);

                        P.Add(A);
                        PF.Add(A);
                    }

                }
                P.Add(new Point((int)(1 * Xm) + Xn, (int)(x * Ym) + Yn));


                g.FillPolygon(B1, P.ToArray());
                if(PF.Count >1) g.DrawLines(P1, PF.ToArray());


                double V = x - Zadeln[i] / Zadelmax[i] / 2;
                double V2 = x - ZadelGraph[i].Last() / Zadelmax[i] / 2;
                g.DrawString(Zadeln[i].ToString("0.00"), drawFont2, drawBrush2, Xn, (int)((V * Ym) + Yn), drawFormat);
                g.DrawString(ZadelGraph[i].Last().ToString("0.00"), drawFont2, drawBrush2, Xn + Xm, (int)((V2 * Ym) + Yn), drawFormat2);
                g.DrawString(Zadelmax[i].ToString("0.00"), drawFont2, drawBrush2, Xn, (int)(((x - 0.5) * Ym) + Yn), drawFormat);


                g.DrawLine(P1, new Point((int)(Xn), (int)(x * Ym) - Yn), new Point((int)(Xn), (int)((x + 1) * Ym) - Yn));
                g.DrawLine(P1, new Point((int)(Xm + Xn), (int)(x * Ym) - Yn), new Point((int)(Xm + Xn), (int)((x + 1) * Ym) - Yn));
            }


            int L = 2;
            for (int i = 0; i < Mach_coun_d.Count; i++)
            {
                if (Mach_coun_d[i] + 1 > L) L = Mach_coun_d[i] + 1;
            }
            for (int i = 0; i < COunt_machiens_time.Count; i++)
            {
                x = 0;

                for (int j = 0; j < Traz.Count - 1; j++)
                {
                    for (int k = 0; k < COunt_machiens_time[i][j]; k++)
                    {
                        x = (double)i + 1;
                        double x2 = -(double)(k) / (double)L + 0.5;
                        Point A = new Point((int)(Traz[j] * Xm) + Xn, (int)(x * Ym + x2 * Ym2) - Yn*2);
                        Point B = new Point((int)(Traz[j + 1] * Xm) + Xn, (int)(x * Ym + x2 * Ym2) - Yn*2);
                        g.DrawLine(P2, A, B);
                    }

                }

            }
        }

        #region SaveLoad
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить как ...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "File (*.txt)|*.txt";
            savedialog.ShowHelp = true;

            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                Save(savedialog.FileName);
            }
        }
        private void Save(String Name)
        {

            System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(Name);

            SaveFile.WriteLine("# TXT fale:");
            SaveFile.WriteLine("# Date:" + System.DateTime.Today.ToString());
            SaveFile.WriteLine("");

            SaveFile.WriteLine("Operations and their time standard min./pc. :");
            int count = Names_operations.Count;
            for (int i = 0; i < count; i++)
            {
                SaveFile.WriteLine("Name:" + " " + Names_operations[i] + "  |Standard:" + " " + (operations_K[i] / 60).ToString());
            }
            SaveFile.WriteLine("");
            SaveFile.WriteLine("time fund: " + F_normal.ToString() + "  - фонд нормы времени");
            SaveFile.WriteLine("reject rate: " + Per_brak.ToString() + "  - процент брака от выпуска");
            SaveFile.WriteLine("release rate: " + Norm_vip.ToString() + "  - норма выпуска");
            SaveFile.WriteLine("");
            SaveFile.WriteLine("Loss of time:");

            count = B_times.Count;
            for (int i = 0; i < count; i++)
            {
                SaveFile.WriteLine("B_" + i.ToString() + " = " + B_times[i].ToString());
            }


            SaveFile.Close();
        }
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "file (*.txt)|*.txt;";
            if (dialog.ShowDialog() == DialogResult.OK)//вызываем диалоговое окно и проверяем выбран ли файл
            {
                load(dialog.FileName);
            }

            InicializePoles();
        }
        private void load(String Name)
        {
            System.IO.FileStream fileStream = new System.IO.FileStream(Name, System.IO.FileMode.Open);
            System.IO.StreamReader reader = new System.IO.StreamReader(fileStream, Encoding.Default);



            Names_operations.Clear();
            operations_K.Clear();
            B_times.Clear();


            String line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("Name:"))
                {
                    String str = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    double d = Double.Parse(line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[3].Replace('.', ','));
                    Names_operations.Add(str);
                    operations_K.Add(d * 60);
                }

                if (line.StartsWith("B_"))
                {
                    double d = Double.Parse(line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2].Replace('.', ','));
                    B_times.Add(d);
                }

                if (line.StartsWith("time fund: "))
                    F_normal = Double.Parse(line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2].Replace('.', ','));
                if (line.StartsWith("reject rate:"))
                    Per_brak = Double.Parse(line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2].Replace('.', ','));
                if (line.StartsWith("release rate: "))
                    Norm_vip = Double.Parse(line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2].Replace('.', ','));
            }
            fileStream.Close();
        }

        #endregion

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog PrintD = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.BeginPrint += pd_BeginPrint;
            doc.PrintPage += pd_PrintPage;

            doc.PrinterSettings.FromPage = 0;
            doc.PrinterSettings.ToPage = 1;

            PrintD.Document = doc;
            if (PrintD.ShowDialog() == DialogResult.OK)
                PrintD.Document.Print();
        }


        int GranX = 100;
        int GranY = 100;

        int indexCuurentPage = 0;
        private void pd_BeginPrint(object sender, PrintEventArgs e)
        {


            tabControl1.TabPages[0].Show();
            tabControl1.TabPages[1].Show();
            tabControl1.TabPages[2].Show();
            tabControl1.TabPages[3].Show();
            tabControl1.TabPages[4].Show();
            tabControl1.TabPages[5].Show();
            tabControl2.TabPages[0].Show();
            tabControl2.TabPages[1].Show();


            indexCuurentPage = 0;
            ((PrintDocument)sender).PrinterSettings.PrintRange = PrintRange.SomePages;
            ((PrintDocument)sender).PrinterSettings.FromPage = 0;
            ((PrintDocument)sender).PrinterSettings.ToPage = 1;
        }


        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {

            Bitmap B;
            e.HasMorePages = true;
            Graphics g = e.Graphics;

            GranY = 40;

            Size S = tabControl1.TabPages[0].Size;
            int YY = S.Height;
            int XX = S.Width;


            switch (indexCuurentPage)
            {
                case 0:
                    {
                        B = new Bitmap(XX, YY);
                        tabControl1.TabPages[0].DrawToBitmap(B, new Rectangle(0, 0, XX, YY));
                        g.DrawImage(B, new Point(GranX, GranY));
                        GranY += YY + 5;
                        B = new Bitmap(XX, YY);
                        tabControl1.TabPages[1].DrawToBitmap(B, new Rectangle(0, 0, XX, YY));
                        g.DrawImage(B, new Point(GranX, GranY));
                        GranY += YY + 5;
                        B = new Bitmap(XX, YY);
                        tabControl1.TabPages[2].DrawToBitmap(B, new Rectangle(0, 0, XX, YY));
                        g.DrawImage(B, new Point(GranX, GranY));
                        GranY += YY + 5;
                        break;
                    }
                case 1:
                    { 
                         B = new Bitmap(XX, YY);
                        tabControl1.TabPages[3].DrawToBitmap(B, new Rectangle(0, 0, XX, YY));
                        g.DrawImage(B, new Point(GranX, GranY));
                        GranY += YY + 5;

                         S = tabControl2.TabPages[0].Size;
                         YY = S.Height;
                         XX = S.Width;

                        B = new Bitmap(XX, YY);
                        tabControl2.TabPages[0].DrawToBitmap(B, new Rectangle(0, 0, XX, YY));
                        g.DrawImage(B, new Point(GranX, GranY));
                        GranY += YY + 5;
                        B = new Bitmap(XX, YY);
                        tabControl2.TabPages[1].DrawToBitmap(B, new Rectangle(0, 0, XX, YY));
                        g.DrawImage(B, new Point(GranX, GranY));

                        e.HasMorePages = false;
                        break;
                    }
            }
            indexCuurentPage++;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void Layer_Picture_Click(object sender, EventArgs e)
        {

        }

        private void сведенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_info_application F = new Form_info_application();
            F.Show();
        }
    }
}
