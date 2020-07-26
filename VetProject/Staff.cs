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
    public partial class Staff : Form
    {
        public Staff()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateStaff st = new CreateStaff(this);
            st.ShowDialog();
        }

        MySqlConnection conn = DB.getConnect();
        MySqlDataAdapter da;
        MySqlCommand cmd;
        MySqlDataReader dr;
        DataSet ds = new DataSet();
        string sql = "";

        private void Staff_Load(object sender, EventArgs e)
        {
            ShowData();
        }

        public void ShowData()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select * from staff";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["ID"].ToString(), dr["name"].ToString() +" "+ dr["surname"].ToString(), dr["job"].ToString(), dr["tel"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;

            for (int j = 0; j <= dataGridView1.RowCount - 1; j++)
            {
                DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)dataGridView1.Rows[j].Cells[5];
                buttonCell.FlatStyle = FlatStyle.Flat;
                buttonCell.Style.BackColor = System.Drawing.Color.FromArgb(0, 192, 239);
                buttonCell.Style.Font = new Font("Mongolian Baiti", 12);
                buttonCell.Style.ForeColor = Color.White;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            sql = "select * from staff where name like '%" + textBox1.Text + "%' or surname like '%" + textBox1.Text + "%' or job like '%" + textBox1.Text + "%'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["ID"].ToString(), dr["name"].ToString() + " " + dr["surname"].ToString(), dr["job"].ToString(), dr["tel"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column4")
            {
                CreateStaff st = new CreateStaff(this);
                st.OpenData(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                st.ShowDialog();
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
        }
    }
}
