using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fabrication_calculation
{
    public partial class Form_add_B_time : Form
    {
        Form1 Main_Form;

        public Form_add_B_time(Form1 Main_Form_l)
        {
            Main_Form = Main_Form_l;

            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double Value = 0;
            textBox1.Text = textBox1.Text.Replace('.', ',');

            try
            {
                NumberStyles style = NumberStyles.Float;
                IFormatProvider provider = CultureInfo.CreateSpecificCulture("fr-FR");
                Value = Double.Parse(textBox1.Text, style, provider);
            }
            catch { }
            try
            {
                NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
               | NumberStyles.AllowParentheses;
                IFormatProvider provider = NumberFormatInfo.InvariantInfo;
                Value = Double.Parse(textBox1.Text, style, provider);
            }
            catch { }

            if (Value !=0 ) Main_Form.add_B_time(Value);
            this.Close();
        }
    }
}
