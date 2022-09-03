using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Fabrication_calculation
{
    public partial class Form_add_operation : Form
    {
        Form1 Main_Form;

        public Form_add_operation(Form1 Main_Form_l, int Operation_Num,int Def_type)
        {
            Main_Form = Main_Form_l;
            InitializeComponent();
            textBox1.Text = Operation_Num.ToString();
            comboBox2.SelectedIndex = Def_type;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            double Value = 0;
            textBox2.Text = textBox2.Text.Replace('.', ',');

            try
            {
                NumberStyles  style = NumberStyles.Float;
                IFormatProvider provider = CultureInfo.CreateSpecificCulture("fr-FR");
                Value = Double.Parse(textBox2.Text, style, provider);
            }
            catch { }
            try
            {
                NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
               | NumberStyles.AllowParentheses;
                IFormatProvider provider = NumberFormatInfo.InvariantInfo;
                Value = Double.Parse(textBox2.Text, style, provider);
            }
            catch { }

            switch (comboBox2.SelectedIndex)
            {
                case 1: { Value *= 60;  break; }
                case 2: { Value *= 3600; break; }
                default: break;
            }
            if (Value != 0) Main_Form.Add(textBox1.Text, Value);
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Main_Form.defoult_select_type_add_operation = comboBox2.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
