using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VetProject
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        string sql = "";
        DataSet ds = new DataSet();
        
        private void button1_Click(object sender, EventArgs e)
        {
            sql = "select * from staff where username = '" + textBox1.Text + "' and password = '" + textBox2.Text + "'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                GetID.staffID = dr["ID"].ToString();
                if (dr["status"].ToString() == "Admin")
                {

                    Admin c = new Admin();
                    c.Show();
                }
                else if (dr["status"].ToString() == "User")
                {
                    Home h = new Home();
                    h.Show();
                }
                textBox1.Text = "";
                textBox2.Text = "";
                checkBox1.Checked = false;
            }
            else
            {
                MessageBox.Show("Your username and password is invalid");
            }
            dr.Close();
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            SwitchLanguage.setLanguage();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            
        }
    }
}
