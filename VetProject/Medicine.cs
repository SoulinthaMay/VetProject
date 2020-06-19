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
    public partial class Medicine : Form
    {
        public Medicine()
        {
            InitializeComponent();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataSet ds = new DataSet();
        MySqlDataReader dr;
        string sql = "";

        private void ShowData()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select * from medicine";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["ID"].ToString(), dr["name"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "   Save")
            {
                sql = "insert into medicine(name) values (@name)";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    ShowData();
                }
            }
            else if (button1.Text == "   Update")
            {
                sql = "update medicine set name=@name where ID = '"+id+"'";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    ShowData();
                }
            }
            
        }

        //Unit
        private void ShowUnit()
        {
            dataGridView2.Rows.Clear();
            int i = 0;
            sql = "select * from unit";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr["unitID"].ToString(), dr["unit"].ToString());
            }
            dr.Close();
            dataGridView2.Columns[1].Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "   Save")
            {
                sql = "insert into unit(unit) values ('"+textBox2.Text+"')";
                cmd = new MySqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    ShowUnit();
                }
            }
            else if (button4.Text == "   Update")
            {
                sql = "update unit set unit = '" + textBox2.Text + "' where unitID = '"+unitID+"'";
                cmd = new MySqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Save data successfully");
                    ShowUnit();
                }
            }
        }

        private void Medicine_Load(object sender, EventArgs e)
        {
            ShowData();
            ShowUnit();
        }

        string unitID = "";

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            unitID = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            button4.Text = "   Update";

            if (dataGridView2.Columns[e.ColumnIndex].Name == "Column9")
            {
                DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        sql = "delete from unit where unitID = '" + unitID + "'";
                        cmd = new MySqlCommand(sql, conn);
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Delete data successfully");
                            ShowUnit();
                            textBox2.Text = "";
                            button4.Text = "   Save";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ບໍ່ສາມາດລຶບຂໍ້ມູນຊຸດນີ້ໄດ້ ເພາະຖືກນຳໃຊ້ໃນຕາຕະລາງອື່ນ");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            button4.Text = "   Save";
        }

        string id = "";

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            button1.Text = "   Update";
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Columns5")
            {
                DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    sql = "delete from medicine where ID = '"+id+"'";
                    cmd = new MySqlCommand(sql, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Delete data successfully");
                        ShowData();
                        button1.Text = "   Save";
                        textBox1.Text = "";
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Text = "   Save";
            textBox1.Text = "";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select * from medicine where name like '%"+txtSearch.Text+"%'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["ID"].ToString(), dr["name"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
