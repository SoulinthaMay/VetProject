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
    public partial class Type : Form
    {
        public Type()
        {
            InitializeComponent();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlDataAdapter da;
        MySqlCommand cmd;
        MySqlDataReader dr;
        DataSet ds = new DataSet();
        string sql = "";

        private void ShowData()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select * from type";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["typeID"].ToString(), dr["type"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "   Save")
            {
                sql = "insert into type(type) values ('" + txtName.Text + "')";
                cmd = new MySqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    ShowData();
                }
            }
            else if (button1.Text == "   Update")
            {
                sql = "update type set type = '" + txtName.Text + "' where typeID = '" + id + "'";
                cmd = new MySqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    ShowData();
                }
            }
            
        }

        private void Type_Load(object sender, EventArgs e)
        {
            ShowData();
            SwitchLanguage.setLanguage();
        }

        string id = "";
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            button1.Text = "   Save";
        }

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
                    sql = "delete from type where typeID = '" + id + "'";
                    cmd = new MySqlCommand(sql, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Delete data successfully");
                        ShowData();

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
            sql = "select * from type where type like '%"+textBox1.Text+"%'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["typeID"].ToString(), dr["type"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = SwitchLanguage.la;
        }
    }
}
