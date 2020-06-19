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
    public partial class RecordReport : Form
    {
        public RecordReport()
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
            sql = "select A.ID, B.name, C.type, D.name as vet, E.name as treatment, DATE_FORMAT(A.date, '%d-%m-%Y') as date from record A inner join patient B on A.patientID = B.ID inner join type C on B.typeID = C.typeID inner join staff D on A.vetID = D.ID inner join treatment E on A.treatmentID = E.ID where A.date >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and A.date <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
            da = new MySqlDataAdapter(sql, conn);
            da.Fill(ds, "1");
            if (ds.Tables["1"] != null)
            {
                ds.Tables["1"].Clear();
            }
            da.Fill(ds, "1");

            ReportDocument rpt = new ReportDocument();
            rpt.Load("C:/Users/Soulintha/Desktop/VetProject/VetProject/ReportRp.rpt");
            rpt.SetDataSource(ds.Tables["1"]);
            rpt.SetParameterValue("date1", dateTimePicker1.Value.ToString("dd-MM-yyyy"));
            rpt.SetParameterValue("date2", dateTimePicker2.Value.ToString("dd-MM-yyyy"));
            this.crystalReportViewer1.ReportSource = rpt;
            this.crystalReportViewer1.Refresh();
        }
    }
}
