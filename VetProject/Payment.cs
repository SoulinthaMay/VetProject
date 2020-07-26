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
    public partial class Payment : Form
    {
        public Payment()
        {
            InitializeComponent();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataReader dr;
        String sql = "";

        public void ShowForm(Form a)
        {
            pn1.Controls.Clear();
            a.TopLevel = false;
            a.Parent = this.pn1;
            a.BringToFront();
            a.Show();
        }

        int total = 0;
        string treatmentID = "";
        private void ShowTreatment()
        {
            int i = 0;
            int treatment = 0;
            sql = "select A.recordID, B.name, A.price, C.total from recorddetail A inner join treatment B on A.treatmentID = B.ID inner join record C on A.recordID = C.recordID where C.patientID = '" + GetPatientID.patientID + "' and C.status = 'NOT PAID'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["recordID"].ToString(), dr["name"].ToString(), string.Format("{0:#,0 ກີບ}", int.Parse(dr["price"].ToString())));

                treatmentID = dr["recordID"].ToString();
                treatment += int.Parse(dr["price"].ToString());
                lbTreatment.Text = string.Format("{0:#,0 ກີບ}", treatment);
            }
            dr.Close();
            total += treatment;
        }

        string medicineID = "";
        private void ShowMedicine()
        {
            int i = 0;
            int medicine = 0;
            sql = "select A.ID, concat(C.name, ' (', D.unit, ') x', B.quantity) as name, B.price, A.total from prescription A inner join prescriptdetail B on A.ID = B.preID inner join medicine C on B.medID = C.ID inner join unit D on C.unitID = D.unitID where A.patientID = '" + GetPatientID.patientID + "' and A.status = 'NOT PAID'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["ID"].ToString(), dr["name"].ToString(), string.Format("{0:#,0 ກີບ}", int.Parse(dr["price"].ToString())));
                medicineID = dr["ID"].ToString();
                medicine += int.Parse(dr["price"].ToString());
                lbMedicine.Text = string.Format("{0:#,0 ກີບ}", medicine);
            }
            dr.Close();
            total += medicine;
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            ShowTreatment();
            ShowMedicine();
            lbTotal.Text = string.Format("{0:#,0 ກີບ}", total);
            SwitchLanguage.setLanguage();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BillTreatment b = new BillTreatment(this);
            b.ShowData(treatmentID, medicineID, lbTotal.Text);
            b.Show();

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            NumberOnly.setNumber(sender, e, textBox1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (total == 0)
            {
                return;
            }

            if (textBox1.Text == "")
            {
                MessageBox.Show("ກະລຸນາປ້ອນຈຳນວນເງິນ");
                return;
            }
            int money = int.Parse(textBox1.Text);
            if ( money < total)
            {
                MessageBox.Show("ຮັບເງິນມາບໍ່ພໍ");
                return;
            }
            int change = money - total;
            lbChange.Text = string.Format("{0:#,0 ກີບ}", change);


                sql = "update record set status = @status where recordID = @ID";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("status", "PAID");
                cmd.Parameters.AddWithValue("ID", treatmentID);

                if (cmd.ExecuteNonQuery() == 1)
                {

                }
            
                sql = "update prescription set status = @status where ID = @ID";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("status", "PAID");
                cmd.Parameters.AddWithValue("ID", medicineID);

                if (cmd.ExecuteNonQuery() == 1)
                {

                }
            MessageBox.Show("ການຊຳລະຄ່າຮັກສາສຳເລັດ");
            btnPrint.Enabled = true;
            button1.Enabled = false;
        }
    }
}
