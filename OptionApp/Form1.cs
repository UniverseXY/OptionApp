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

namespace OptionApp
{
    public partial class Form1 : Form
    {
        Option option;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string N = NText.Text.Trim(' ');
            string B0 = B0Text.Text.Trim(' ');
            string S0 = S0Text.Text.Trim(' ');
            string K = KText.Text.Trim(' ');
            string a = aText.Text.Trim(' ');
            string b = bText.Text.Trim(' ');
            string r = rText.Text.Trim(' ');
            if (N == "" || B0 == "" || S0 == "" || K == "" || a == "" || b == "" || r == "")
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK , MessageBoxIcon.Error);
            else
            {
                 option = new Option(Convert.ToInt32(N), Convert.ToDouble(B0),
                 Convert.ToDouble(S0), Convert.ToDouble(K), Convert.ToDouble(a),
                 Convert.ToDouble(r), Convert.ToDouble(b));
                 MessageBox.Show("Рациональная стоимость опциона равна " + Convert.ToString(option.EvaluateRationalPrice()),
                    "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.None);
                 button2.Visible = true;
                
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(option);
            button2.Visible = false;
            form2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NText.Text = "";
            B0Text.Text = "";
            S0Text.Text = "";
            KText.Text = "";
            aText.Text = "";
            bText.Text = "";
            rText.Text = "";
            button2.Visible = false;
        }
    }
}
