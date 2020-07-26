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
    public partial class CreatePatient : Form
    {
        public CreatePatient()
        {
            InitializeComponent();
        }

        Patient _p;
        public CreatePatient(Patient p)
        {
            InitializeComponent();
            _p = p;
        }

        MedicalRecord _m;
        public CreatePatient(MedicalRecord m)
        {
            InitializeComponent();
            _m = m;
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        DataSet ds = new DataSet();
        string sql = "";

        private void insertupdate()
        {
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", txtName.Text);
            cmd.Parameters.AddWithValue("typeID", cbxSpecies.SelectedValue);
            cmd.Parameters.AddWithValue("gender", cbxGender.Text);
            cmd.Parameters.AddWithValue("age", txtAge.Text);
            cmd.Parameters.AddWithValue("weight", txtWeight.Text);
            cmd.Parameters.AddWithValue("owner", txtOwner.Text);
            cmd.Parameters.AddWithValue("tel", txtTel.Text);

            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Save data successfully");
                _p.ShowData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtAge.Text == "   y    m    d" || txtName.Text == "" || txtWeight.Text == "" || txtOwner.Text == "" || txtTel.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            if(button1.Text == "   Save")
            {
                sql = "insert into patient(name, typeID, gender, age, weight, owner, tel) values (@name, @typeID, @gender, @age, @weight, @owner, @tel)";
                insertupdate();
                this.Close();
            }
           else if(button1.Text == "   Update")
            {
                sql = "update patient set name = @name, typeID = @typeID, gender = @gender, age = @age, weight = @weight, owner = @owner, tel = @tel where ID = '"+petID+"'";
                insertupdate();
            }
        }

        private void CreatePatient_Load(object sender, EventArgs e)
        {
            ShowType();
            SwitchLanguage.setLanguage();
            setID(petID);
        }

        private void ShowType()
        {
            sql = "select * from type";
            da = new MySqlDataAdapter(sql, conn);
            da.Fill(ds, "type");
            if (ds.Tables["type"] != null)
            {
                ds.Tables["type"].Clear();
            }
            da.Fill(ds, "type");
            cbxSpecies.DataSource = ds.Tables["type"];
            cbxSpecies.DisplayMember = "type";
            cbxSpecies.ValueMember = "typeID";
        }

        string petID = "";
        public void setID(string patientID)
        {
            if (patientID == "")
            {
                return;
            }

            petID = patientID;
            sql = "select A.ID, A.name, B.type, A.gender,A.age, A.weight, A.owner,  A.tel from patient A inner join type B on A.typeID = B.typeID where ID = '" + petID + "'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                txtName.Text = dr["name"].ToString();
                cbxSpecies.Text = dr["type"].ToString();
                cbxGender.Text = dr["gender"].ToString();
                txtAge.Text = dr["age"].ToString();
                txtWeight.Text = dr["weight"].ToString();
                txtOwner.Text = dr["owner"].ToString();
                txtTel.Text = dr["tel"].ToString();
            }
            dr.Close();
            button1.Text = "   Update";
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    sql = "delete from patient where ID = '" + petID + "'";
                    cmd = new MySqlCommand(sql, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Delete data successfully");
                        _p.ShowData();
                    }
                    button1.Text = "   Save";
                    txtName.Text = "";
                    txtAge.Text = "";
                    txtOwner.Text = "";
                    txtTel.Text = "";
                    txtWeight.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ບໍ່ສາມາດລຶບຂໍ້ມູນຊຸດນີ້ໄດ້ ເພາະຖືກນຳໃຊ້ໃນຕາຕະລາງອື່ນ");
                }
            }
        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtWeight_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }

        private void txtAge_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }

        private void txtTel_KeyUp(object sender, KeyEventArgs e)
        {
            NumberOnly.setNumber(sender, e, txtTel);
        }
    }
}
