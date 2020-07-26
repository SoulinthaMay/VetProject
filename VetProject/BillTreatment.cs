using CrystalDecisions.CrystalReports.Engine;
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
    public partial class BillTreatment : Form
    {
        public BillTreatment()
        {
            InitializeComponent();
        }

        Payment _p;
        public BillTreatment(Payment p)
        {
            InitializeComponent();
            _p = p;
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        DataSet ds = new DataSet();
        string sql = "";
        
        
        public void ShowData(string treID, string medID, string total)
        {
            string owner = "";
            string patient = "";
            string type = "";
            string date = "";
            sql = "SELECT B.name, B.owner, C.type, DATE_FORMAT(A.date, '%d-%m-%Y') as date FROM `record` A inner join patient B on A.patientID = B.ID inner join type C on B.typeID = C.typeID where A.recordID = '"+treID+"' union all select E.name, E.owner, F.type, DATE_FORMAT(D.date, '%d-%M-%y') as date from prescription D inner join patient E on D.patientID = E.ID inner join type F on E.typeID = F.typeID where D.ID = '"+medID+"'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                owner = dr["owner"].ToString();
                patient = dr["name"].ToString();
                type = dr["type"].ToString();
                date = dr["date"].ToString();

            }
            dr.Close();

            sql = "SELECT C.name, concat(B.price, ' ກີບ') as price FROM record A inner join recorddetail B on A.recordID = B.recordID inner join treatment C on B.treatmentID = C.ID where A.recordID = @recordID UNION ALL SELECT  concat(F.name, ' (', I.unit, ') x', E.quantity) as name, concat(E.price, ' ກີບ') as price FROM prescription D inner join prescriptdetail E on D.ID = E.preID inner join medicine F on E.medID = F.ID inner join unit I on F.unitID = I.unitID WHERE D.ID = @preID";
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("recordID", treID);
            cmd.Parameters.AddWithValue("preID", medID);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "bill");
            if (ds.Tables["bill"] != null)
            {
                ds.Tables["bill"].Clear();
            }
            da.Fill(ds, "bill");

            ReportDocument rpt = new ReportDocument();
            rpt.Load("C:/Users/Soulintha/Desktop/VetProject/VetProject/BillReport.rpt");
            rpt.SetDataSource(ds.Tables["bill"]);
            rpt.SetParameterValue("owner", owner);
            rpt.SetParameterValue("date", date);
            rpt.SetParameterValue("patient", patient);
            rpt.SetParameterValue("type", type);
            rpt.SetParameterValue("total", total);
            this.crystalReportViewer1.ReportSource = rpt;
            this.crystalReportViewer1.Refresh();
        }
        private void BillTreatment_Load(object sender, EventArgs e)
        {
            
        }
    }
}
