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
    public partial class ReportPatient : Form
    {
        public ReportPatient()
        {
            InitializeComponent();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        DataSet ds = new DataSet();
        string sql = "";

        private void ReportPatient_Load(object sender, EventArgs e)
        {
            //sql = "select A.recordID, concat(A.patientID, 'record'), B.name, C.type from record A inner join patient B on A.patientID = B.ID inner join type C on B.typeID = C.typeID"

            //How many time patient come to clinic
            //sql = "select recordID from record union select ID from prescription";


            //How many patient in each species
            //sql = "select A.typeID, A.type, count(B.typeID) from type A left join patient B on A.typeID = B.typeID group by A.typeID"

            //How many patient(in species use in record)
            //sql = "select A.typeID, A.type, count(B.typeID) from type A left join patient B on A.typeID = B.typeID left join record C on B.ID = C.patientID group by A.typeID";

            //NO ZERO Species
            //sql = "select C.type, COUNT(A.typeID) from patient A inner join record B on A.ID  = B.patientID inner join type C on A.typeID = C.typeID group by A.typeID having COUNT(A.typeID)>1"

            //Hell it worked
           
        }

        private void ReportPatient_Load_1(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateFrom.Value.Date > dateTo.Value.Date)
            {
                MessageBox.Show("ກະລຸນາເລືອກວັນທີອີກຄັ້ງ");
                return;
            }

            string total = "";
            sql = "select count(recordID) as total from record where date >= '" + dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <= '" + dateTo.Value.ToString("yyy-MM-dd") + "'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                total = dr["total"].ToString();
            }
            dr.Close();

            sql = "select F.type as patientID, (select COUNT(B.typeID) from record A join patient B on A.patientID  = B.ID where B.typeID = F.typeID and A.date >= '"+dateFrom.Value.ToString("yyyy-MM-dd")+"' and A.date <= '"+dateTo.Value.ToString("yyy-MM-dd")+"') as total from type F";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "recordPatient");
            if (ds.Tables["recordPatient"] != null)
            {
                ds.Tables["recordPatient"].Clear();
            }
            da.Fill(ds, "recordPatient");

            ReportDocument rpt = new ReportDocument();
            rpt.Load("C:/Users/Soulintha/Desktop/VetProject/VetProject/CrystalReport1.rpt");
            rpt.SetDataSource(ds.Tables["recordPatient"]);
            rpt.SetParameterValue("date1", dateFrom.Value.ToString("dd-MM-yyyy"));
            rpt.SetParameterValue("date2", dateTo.Value.ToString("dd-MM-yyyy"));
            rpt.SetParameterValue("total", total);
            this.crystalReportViewer1.ReportSource = rpt;
            this.crystalReportViewer1.Refresh();
        }
    }
}
