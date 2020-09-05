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
    public partial class addTreatment : Form
    {
        public addTreatment()
        {
            InitializeComponent();
        }

        Record _r;
        public addTreatment(Record r)
        {
            InitializeComponent();
            _r = r;
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataSet ds = new DataSet();
        MySqlDataReader dr;
        string sql = "";

        private void addTreatment_Load(object sender, EventArgs e)
        {
            ShowType();
            SwitchLanguage.setLanguage();
        }

        private void ShowTreatment()
        {
            sql = "select * from treatment where typeID = '" + comboBox3.SelectedValue.ToString() + "'";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "t");
            if (ds.Tables["t"] != null)
            {
                ds.Tables["t"].Clear();
            }
            da.Fill(ds, "t");
            comboBox1.DataSource = ds.Tables["t"];
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "ID";


        }


        private void ShowType()
        {
            sql = "select * from treatmenttype";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "note");
            if (ds.Tables["note"] != null)
            {
                ds.Tables["note"].Clear();
            }
            da.Fill(ds, "note");
            comboBox3.DataSource = ds.Tables["note"];
            comboBox3.DisplayMember = "name";
            comboBox3.ValueMember = "ID";

            ShowTreatment();

            ShowPrice();
        }


        private void ShowPrice()
        {
            sql = "select priceRange from treatment where ID = '" + comboBox1.SelectedValue.ToString() + "'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                lbPrice.Text = dr["priceRange"].ToString();
            }
            dr.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtPrice.Text == "")
            {
                MessageBox.Show("ກະລຸນາປ້ອນລາຄາ");
                return;
            }
            _r.fetchData(comboBox1.SelectedValue.ToString(), comboBox3.Text, comboBox1.Text, txtPrice.Text);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowTreatment();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ShowPrice();
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ShowTreatment();
            ShowPrice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtPrice.Text = "";
            lbPrice.Text = "";
        }

        private void txtPrice_KeyUp(object sender, KeyEventArgs e)
        {
            NumberOnly.setNumber(sender, e, txtPrice);
        }

        private void txtPrice_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }
    }
}
