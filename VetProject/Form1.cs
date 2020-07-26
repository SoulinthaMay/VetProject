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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        DataSet ds = new DataSet();
        string sql = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            sql = "SELECT row_number() over (order by A.recordID) as num, C.name, B.price FROM record A inner join recorddetail B on A.recordID = B.recordID inner join treatment C on B.treatmentID = C.ID where A.recordID = 32 UNION ALL SELECT  row_number() over (order by D.ID) as num ,concat(F.name, ' (', I.unit, ') x', E.quantity) as name, E.price FROM prescription D inner join prescriptdetail E on D.ID = E.preID inner join medicine F on E.medID = F.ID inner join unit I on F.unitID = I.unitID WHERE D.ID = 11";
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("recordID", 32);
            cmd.Parameters.AddWithValue("preID", 9);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "bill");
            if (ds.Tables["bill"] != null)
            {
                ds.Tables["bill"].Clear();
            }
            da.Fill(ds, "bill");
            dataGridView1.DataSource = ds.Tables["bill"];
        }
    }
}
