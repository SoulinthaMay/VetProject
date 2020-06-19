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
    public partial class MedicalRecord : Form
    {
        public MedicalRecord()
        {
            InitializeComponent();
        }

        Patient _p;
        public MedicalRecord(Patient p)
        {
            InitializeComponent();
            _p = p;
        }
        

        string ID = "";

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        DataSet ds = new DataSet();
        string sql = "";

        public void setID(string id)
        {
            ID = id;
            sql = "select A.ID , A.name, B.type from patient A inner join type B on A.typeID = B.typeID where ID = '"+id+"'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                lbName.Text = dr["name"].ToString();
                lbID.Text = dr["ID"].ToString();
                lbType.Text = dr["type"].ToString();
            }
            dr.Close();
        }

        private void ShowForm(Form a)
        {
            panel3.Controls.Clear();
            a.TopLevel = false;
            panel3.Controls.Add(a);
            a.Show();
        }

        private void MedicalRecord_Load(object sender, EventArgs e)
        {
            Record r = new Record(this);
            r.patient(lbID.Text);
            ShowForm(r);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Record r = new Record(this);
            r.patient(lbID.Text);
            ShowForm(r);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Prescription p = new Prescription();
            p.patient(lbID.Text);
            ShowForm(p);
        }
        
    }
}
