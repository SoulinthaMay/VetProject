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
using System.IO;

namespace VetProject
{
    public partial class CreateStaff : Form
    {
        public CreateStaff()
        {
            InitializeComponent();
        }

        Staff _s;
        public CreateStaff(Staff s)
        {
            InitializeComponent();
            _s = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.InitialDirectory = @"C:\Users\Soulintha\Desktop\Pet Clinic\Clinic\Staff";
            DialogResult result = opfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                pictureBox1.Load(opfd.FileName);
            }
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        DataSet ds = new DataSet();
        string sql = "";

        private void insertupdate()
        {
            try
            {
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("name", txtName.Text);
                cmd.Parameters.AddWithValue("surname", txtSurname.Text);
                cmd.Parameters.AddWithValue("user", txtUser.Text);
                cmd.Parameters.AddWithValue("pass", txtPass.Text);
                cmd.Parameters.AddWithValue("job", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("tel", txtTel.Text);
                cmd.Parameters.AddWithValue("status", cbStatus.Text);

                MemoryStream mem = new MemoryStream();
                pictureBox1.Image.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                byte[] arrpic = mem.ToArray();
                cmd.Parameters.AddWithValue("pic", arrpic);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    _s.ShowData();
                    this.Close();
                }
            }
            catch (Exception e)
            {

                //MessageBox.Show("ຮູບມີຂະໜາດໃຫຍ່ເກີນໄປ");
                MessageBox.Show(e.Message.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtSurname.Text == "" || txtTel.Text == "" || txtTel.Text.Length < 15)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            if (txtPass.Text.Length < 5)
            {
                MessageBox.Show("ລະຫັດຜ່ານຕ້ອງມີຫຼາຍກວ່າ ຫຼື ເທົ່າກັບ 5 ໂຕ");
                return;
            }

            if (button3.Text == "   Save")
            {
                sql = "insert into staff (name, surname,username, password, job, tel, pic, status) values (@name, @surname, @user, @pass ,@job, @tel, @pic, @status)";
                insertupdate();
                
            }
            else if (button3.Text == "   Update")
            {
                sql = "update staff set name = @name, surname = @surname, username = @user, password = @pass, job = @job, tel = @tel, pic = @pic where ID = '"+a+"'";
                insertupdate();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                sql = "delete from staff where ID = '" + a + "'";
                cmd = new MySqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Delete data successfully");
                    _s.ShowData();
                }

                txtName.Text = "";
                txtSurname.Text = "";
                txtUser.Text = "";
                txtPass.Text = "";
                txtTel.Text = "";
                pictureBox1.Image = null;

                button3.Text = "   Save";
                button2.Enabled = false;
            }
        }

        private void showJob()
        {
            sql = "select * from job";
            da = new MySqlDataAdapter(sql,conn);
            da.Fill(ds, "job");
            if (ds.Tables["job"] != null)
            {
                ds.Tables["job"].Clear();
            }
            da.Fill(ds, "job");
            comboBox1.DataSource = ds.Tables["job"];
            comboBox1.DisplayMember = "jobName";
            comboBox1.ValueMember = "jobID";
        }

        private void CreateStaff_Load(object sender, EventArgs e)
        {
            cbStatus.Text = cbStatus.Items[0].ToString();
            showJob();
            SwitchLanguage.setLanguage();
            if (a != "")
            {
                OpenData(a);
            }
        }

        string a = "";
        public void OpenData(string id)
        {
            a = id;
            sql = "select A.ID, A.name, A.surname, A.username, A.password, B.jobName as job, A.tel, A.status, A.pic from staff A inner join job B on A.job = B.jobID where ID = '"+id+"'";
            cmd = new MySqlCommand(sql, conn);
            //ໃຊ້ DataReader ຍ້ອນເລືອກຂໍ້ມູນພຽງໂຕດຽວ
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                txtName.Text = dr["name"].ToString();
                txtSurname.Text = dr["surname"].ToString();
                txtUser.Text = dr["username"].ToString();
                txtPass.Text = dr["password"].ToString();
                comboBox1.Text = dr["job"].ToString();
                txtTel.Text = dr["tel"].ToString();
                cbStatus.Text = dr["status"].ToString();

                byte[] pic = (byte[])dr["pic"];
                MemoryStream mem1 = new MemoryStream(pic);
                pictureBox1.Image = new Bitmap(mem1);

                button2.Enabled = true;
                button3.Text = "   Update";
            }
            dr.Close();
        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.la;
        }

        private void txtTel_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.en;
        }
    }
}
