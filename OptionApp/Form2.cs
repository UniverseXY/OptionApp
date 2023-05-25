using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptionApp
{
    public partial class Form2 : Form
    {
        Option option;
        public Form2(Option option)
        {
            InitializeComponent();
            button2.Visible = false;
            this.option = option;
            MakeStep(option.isLastStep);
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            if (option.isLastStep)
            {
                MessageBox.Show("Это последний шаг!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                option.StepUp(false);
                MakeStep(option.isLastStep);
                button2.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (option.isLastStep)
            {
                MessageBox.Show("Это последний шаг!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                option.StepUp(true);
                MakeStep(option.isLastStep);
                button2.Visible = true;
            }
        }

        private void MakeStep(bool isLastStep) {
            option.RedistributePortfolio();
            label10.Text = Convert.ToString(option.GetCurrentGamma());
            label11.Text = Convert.ToString(option.GetCurrentBeta());
            label9.Text = Convert.ToString(option.GetCurrentStep());
            label4.Text = Convert.ToString(option.GetSPrice());
            label7.Text = Convert.ToString(option.GetB());
            if (isLastStep)
            {
                var payoutToClient = option.EvaluatePayoutFunction(option.GetSPrice());
                var payoutToBank = option.GetSPrice() * option.GetCurrentGamma() - payoutToClient;
                MessageBox.Show("Мы должны выплатить предъявителю опциона " + payoutToClient + " у.е. В банк мы должны вернуть " +
                    payoutToBank + " у.е.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else
            {
                if (option.GetCurrentStep() == 0)
                {   
                    MessageBox.Show("Берем в долг " + Math.Abs(option.GetCurrentBeta()) + " облигаций по текущей цене " + option.GetB() + " у.е. на общую сумму " +
                       Math.Abs(option.GetB() * option.GetCurrentBeta()) + " у.е. Всю эту сумму вкладываем в акции.", "Сообщение",
                        MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else if (option.GetCurrentGamma() == 0 && option.GetCurrentBeta() == 0)
                {
                    MessageBox.Show("Продаем все свои акции в количестве " + Math.Abs(option.GetPrevGamma()) + " на общую сумму " +
                        + option.GetSPrice() * Math.Abs(option.GetPrevGamma()) + " у.е. и возвращаем долг в банк.", "Сообщение",
                        MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show("Берем в долг " + Math.Abs(option.GetCurrentBeta() - option.GetPrevBeta()) + 
                        " облигаций по текущей цене " + option.GetB() + " у.е на общую сумму " +
                      option.GetB() * Math.Abs(option.GetCurrentBeta() - option.GetPrevBeta()) + " у.е. Всю эту сумму вкладываем в акции.", "Сообщение",
                        MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (option.GetCurrentStep() == 0)
            {
                MessageBox.Show("Предыдущего шага нет!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                option.StepDown();
                MakeStep(false);
            }
        }
    }
}
