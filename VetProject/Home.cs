using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VetProject
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        string sql = "";
        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataReader dr;
        private void Form1_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, pictureBox1.Width - 3, pictureBox1.Height - 3);
            Region rg = new Region(gp);
            pictureBox1.Region = rg;

            sql = "select concat(name, ' ', surname) as fullname, pic from staff where ID = '"+GetID.staffID+"'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                lbName.Text = dr["fullname"].ToString();

                byte[] pic = (byte[])dr["pic"];
                MemoryStream mem1 = new MemoryStream(pic);
                pictureBox1.Image = new Bitmap(mem1);

            }
            Patient register = new Patient();
            ShowForm(register);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Patient register = new Patient();
            ShowForm(register);
        }
        
        public void ShowForm(Form a)
        {
            panel3.Controls.Clear();
            a.TopLevel = false;
            a.Parent = this.panel3;
            a.BringToFront();
            a.Show();
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
