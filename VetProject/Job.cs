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
    public partial class Job : Form
    {
        public Job()
        {
            InitializeComponent();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        DataSet ds = new DataSet();
        string sql = "";

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void showJob()
        {
            int i = 0;
            sql = "select * from job";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["jobID"].ToString(), dr["jobName"].ToString());
            }
            dr.Close();
        }

        private void insertupdate()
        {
            cmd = new MySqlCommand(sql, conn);
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Save Data Successfully");
                showJob();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please fill the box");
                return;
            }

            if (button1.Text == "   Save")
            {
                sql = "insert into job values ('" + txtName.Text + "')";
                insertupdate();
            }
            else if (button1.Text == "   Update")
            {
                sql = "update job set jobName = '" + txtName.Text + "'";
                insertupdate();
            }
        }

        private void Job_Load(object sender, EventArgs e)
        {
            showJob();
            SwitchLanguage.setLanguage();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.la;
        }

        string id = "";
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            button1.Text = "   Update";

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column4")
            {
                DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    sql = "delete from job where jobID = '" + id + "'";
                    cmd = new MySqlCommand(sql, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Delete data successfully");
                        showJob();

                        txtName.Text = "";
                        button1.Text = "   Save";
                    }
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select * from job where jobName like '%" + textBox1.Text + "%'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["jobID"].ToString(), dr["jobName"].ToString());
            }
            dr.Close();
        }
    }
}
