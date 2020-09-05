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
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (dateFrom.Value.Date > dateTo.Value.Date)
            {
                MessageBox.Show("ກະລຸນາເລືອກວັນທີອີກຄັ້ງ");
                return;
            }

            sql = "select concat(C.name,' (', D.unit, ')') as name, (select SUM(A.quantity) from prescriptdetail A inner join medicine B on A.medID = B.ID inner join prescription F on A.preID = F.ID where B.ID = C.ID and F.date >= '" + dateFrom.Value.ToString("yyyy-MM-dd") + "' and F.date <= '" + dateTo.Value.ToString("yyyy-MM-dd") + "') as price from medicine C inner join unit D on C.unitID = D.unitID order by price limit 10";
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
            rpt.SetParameterValue("date1", dateFrom.Value.ToString("dd-MM-yyyy"));
            rpt.SetParameterValue("date2", dateTo.Value.ToString("dd-MM-yyyy"));
            this.crystalReportViewer1.ReportSource = rpt;
            this.crystalReportViewer1.Refresh();
        }
    }
}
