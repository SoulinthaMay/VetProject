using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VetProject
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        public void ShowForm(Form a)
        {
            panel3.Controls.Clear();
            a.TopLevel = false;
            a.Parent = this.panel3;
            a.BringToFront();
            panel3.Controls.Add(a);
            a.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Staff st = new Staff();
            ShowForm(st);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            Type t = new Type();
            ShowForm(t);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Treatment t = new Treatment();
            ShowForm(t);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Medicine t = new Medicine();
            ShowForm(t);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.Name=="button8")
            {
                button8.Name = "show";
                radioButton3.Visible = true;
                radioButton2.Visible = true;
               
            }
            else if (button8.Name == "show")
            {
                button8.Name = "button8";
                radioButton3.Visible = false;
                radioButton2.Visible = false;
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                RecordReport r = new RecordReport();
                ShowForm(r);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                PrescriptionReport r = new PrescriptionReport();
                ShowForm(r);
            }
        }
    }
}
