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
    public partial class Treatment : Form
    {
        public Treatment()
        {
            InitializeComponent();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        DataSet ds = new DataSet();
        string sql = "";

        private void Note_Load(object sender, EventArgs e)
        {
            ShowData();
            ShowType();
        }

        private void ShowData()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select A.ID, A.name, B.name as type from treatment A inner join treatmenttype B on A.typeID = B.ID order by typeID asc";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["ID"].ToString(), dr["type"].ToString(), dr["name"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;
        }

        string id = "";

        private void insertupdate()
        {
            cmd = new MySqlCommand(sql, conn);
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Save data successfully");
                ShowData();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "   Save")
            {
                sql = "insert into treatment(name, typeID) values ('" + txtNote.Text + "', '" + comboBox1.SelectedValue.ToString() + "')";
                insertupdate();
            }
            else if (button1.Text == "   Update")
            {
                sql = "update treatment set name = '" + txtNote.Text + "', typeID = '" + comboBox1.SelectedValue.ToString() + "' where ID = '" + id + "'";
                insertupdate();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtNote.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            button1.Text = "   Update";

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column3")
            {
                DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    sql = "delete from treatment where ID = '" + id + "'";
                    cmd = new MySqlCommand(sql, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Delete data successfully");
                        ShowData();

                        txtNote.Text = "";
                        button1.Text = "   Save";
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtNote.Text = "";
            button1.Text = "   Save";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select A.ID, A.name, B.name as type from treatment A inner join treatmenttype B on A.typeID = B.ID where B.name like '%"+textBox1.Text+ "%' or A.name like '%" + textBox1.Text + "%' order by typeID asc";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["ID"].ToString(), dr["type"].ToString(), dr["name"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;
        }

        private void ShowType()
        {
            dataGridView2.Rows.Clear();
            int i = 0;
            sql = "select * from treatmenttype";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["ID"].ToString(), dr["name"].ToString());
            }
            dr.Close();

            da = new MySqlDataAdapter(sql, conn);
            da.Fill(ds, "1");
            if (ds.Tables["1"] != null)
            {
                ds.Tables["1"].Clear();
            }
            da.Fill(ds, "1");
            comboBox1.DataSource = ds.Tables["1"];
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "ID";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "") {
                MessageBox.Show("ກະລຸນາປ້ອນປະເພດການຮັກສາ");
                return;
            }
            if (button4.Text == "   Save")
            {
                sql = "insert into treatmenttype(name) values ('" + textBox2.Text + "')";
                cmd = new MySqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    ShowType();
                }
            }
            else if (button4.Text == "   Update")
            {
                sql = "update treatmenttype set name = '"+textBox2.Text+"' where ID = '" + ID + "'";
                cmd = new MySqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    ShowType();
                }
            }
        }

        string ID = "";
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ID = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            button4.Text = "   Update";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Text = "   Save";
            textBox2.Text = "";
        }
    }
}
