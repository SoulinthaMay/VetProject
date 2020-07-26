using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using MySql.Data.MySqlClient;

namespace VetProject
{
    public partial class PrescriptionReport : Form
    {
        public PrescriptionReport()
        {
            InitializeComponent();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlDataAdapter da;
        DataSet ds = new DataSet();
        string sql = "";

        private void button3_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                MessageBox.Show("ກະລຸນາເລືອກວັນທີອີກຄັ້ງ");
                return;
            }
            sql = "select A.ID, B.name, D.name as medicine, E.unit, C.quantity, DATE_FORMAT(A.date, '%d-%m-%Y') as date from prescription A inner join patient B on A.patientID = B.ID inner join prescriptdetail C on A.ID = C.preID inner join medicine D on C.medID = D.ID inner join unit E on D.unitID = E.unitID where A.date >= '"+dateTimePicker1.Value.ToString("yyyy-MM-dd")+"' and A.date <= '"+dateTimePicker2.Value.ToString("yyyy-MM-dd")+"'";
            da = new MySqlDataAdapter(sql, conn);
            da.Fill(ds, "pre");
            if (ds.Tables["pre"] != null)
            {
                ds.Tables["pre"].Clear();
            }
            da.Fill(ds, "pre");

            ReportDocument rpt = new ReportDocument();
            rpt.Load("C:/Users/Soulintha/Desktop/VetProject/VetProject/PrescriptionRp.rpt");
            rpt.SetDataSource(ds.Tables["pre"]);
            rpt.SetParameterValue("date1", dateTimePicker1.Value.ToString("dd-MM-yyyy"));
            rpt.SetParameterValue("date2", dateTimePicker2.Value.ToString("dd-MM-yyyy"));
            this.crystalReportViewer1.ReportSource = rpt;
            this.crystalReportViewer1.Refresh();
        }
    }
}
