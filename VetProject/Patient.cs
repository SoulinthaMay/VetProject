﻿using System;
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
    public partial class Patient : Form
    {
        public Patient()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            CreatePatient patient = new CreatePatient(this);
            patient.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sql = "";

        private void Check()
        {
            sql = "select patientID from record where status = 'NOT PAID' union all select patientID from prescription where status = 'NOT PAID'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                for(int i = 0; i <= dataGridView1.Rows.Count -1; i++)
                {
                    if (dr["patientID"].ToString() == dataGridView1.Rows[i].Cells[1].Value.ToString())
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(236, 223, 206);
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(222, 105, 48);

                        dataGridView1.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 223, 206);
                        dataGridView1.Rows[i].DefaultCellStyle.SelectionForeColor = Color.FromArgb(222, 105, 48);
                    }
                }
            }
            dr.Close();
        }
        public void ShowData()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select A.ID, A.name, B.type, A.gender, A.owner,  A.tel from patient A inner join type B on A.typeID = B.typeID";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["ID"].ToString(), dr["name"].ToString(), dr["type"].ToString(), dr["gender"].ToString(),dr["owner"].ToString(), dr["tel"].ToString());

            }
            dr.Close();


            for (int j = 0; j <= dataGridView1.RowCount - 1; j++)
            {
                DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)dataGridView1.Rows[j].Cells[7];
                buttonCell.FlatStyle = FlatStyle.Flat;
                buttonCell.Style.BackColor = System.Drawing.Color.FromArgb(0, 192, 239);
                buttonCell.Style.Font = new Font("Mongolian Baiti", 12);
                buttonCell.Style.ForeColor = Color.White;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = @"select A.ID, A.name, B.type, A.gender, A.owner,  A.tel from patient A inner join type B on A.typeID = B.typeID where A.name like '%"+textBox1.Text+"%' or B.type like '%"+textBox1.Text+ "%' or A.owner like '%" + textBox1.Text + "%' or A.tel like '%" + textBox1.Text + "%'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["ID"].ToString(), dr["name"].ToString(), dr["type"].ToString(), dr["gender"].ToString(), dr["owner"].ToString(), dr["tel"].ToString());
            }
            dr.Close();
            dataGridView1.Columns[1].Visible = false;
        }

        private void Patient_Load(object sender, EventArgs e)
        {
            ShowData();
            Check();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column7")
            {
                GetPatientID.patientID = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                CreatePatient c = new CreatePatient(this);
                c.setID(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                c.ShowDialog();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Column8")
            {
                GetPatientID.patientID = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                MedicalRecord m = new MedicalRecord(this);
                m.setID();
                panel2.Controls.Clear();

                m.TopLevel = false;
                panel2.Controls.Add(m);
                m.Show();
            }
        }
    }
}
