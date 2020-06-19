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
    public partial class Record : Form
    {
        public Record()
        {
            InitializeComponent();
        }

        MedicalRecord _m;
        public Record(MedicalRecord m)
        {
            InitializeComponent();
            _m = m;
        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataSet ds = new DataSet();
        MySqlDataReader dr;
        string sql = "";
        
        private void Record_Load(object sender, EventArgs e)
        {
            ShowType();

            CreateHistory();
        }

        private void ShowTreatment()
        {
            sql = "select * from treatment where typeID = '" + comboBox3.SelectedValue.ToString() + "'";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "t");
            if (ds.Tables["t"] != null)
            {
                ds.Tables["t"].Clear();
            }
            da.Fill(ds, "t");
            comboBox1.DataSource = ds.Tables["t"];
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "ID";
        }

        private void ShowType()
        {
            sql = "select * from treatmenttype";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "note");
            if (ds.Tables["note"] != null)
            {
                ds.Tables["note"].Clear();
            }
            da.Fill(ds, "note");
            comboBox3.DataSource = ds.Tables["note"];
            comboBox3.DisplayMember = "name";
            comboBox3.ValueMember = "ID";

            ShowTreatment();
        }

        Color beauty = Color.FromArgb(24, 121, 111);
        private void CreateHistory()
        {
            panel2.Controls.Clear();
            sql = "select A.ID, A.patientID, B.name, A.description, DATE_FORMAT(A.date, '%d-%M-%Y'), A.time from record A inner join treatment B on A.treatmentID = B.ID where A.patientID = '" + patientID + "' order by ID desc";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "history");
            if (ds.Tables["history"] != null)
            {
                ds.Tables["history"].Clear();
            }
            da.Fill(ds, "history");

            for (int i = 0; i <= ds.Tables["history"].Rows.Count - 1; i++)
            {
                Button btnDelete = new Button();
                btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(75)))), ((int)(((byte)(57)))));
                btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(75)))), ((int)(((byte)(57)))));
                btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Location = new System.Drawing.Point(755, 0);
                //btnDelete.Name = "button1";
                btnDelete.Size = new System.Drawing.Size(93, 50);
                btnDelete.TabIndex = 2;
                btnDelete.Text = "Delete";
                btnDelete.UseVisualStyleBackColor = false;
                btnDelete.TextAlign = ContentAlignment.MiddleCenter;
                btnDelete.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
                //btnDelete.Click += new EventHandler(btnDelete_Click);



                // 
                // label2
                // 
                Label topic = new Label();
                topic.AutoSize = true;
                topic.Font = new System.Drawing.Font("Noto Serif Lao", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                topic.Location = new System.Drawing.Point(13, 11);
                //topic.Name = "label2";
                topic.Size = new System.Drawing.Size(71, 29);
                topic.TabIndex = 0;
                topic.Text = ds.Tables["history"].Rows[i][2].ToString();
                topic.Name = ds.Tables["history"].Rows[i][0].ToString();
                btnDelete.Click += (sender, e) =>
                {
                    DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        sql = "delete from record where ID = '" + topic.Name + "'";
                        cmd = new MySqlCommand(sql, conn);
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Delete data successfully");
                            CreateHistory();
                        }
                    }
                };

                // 
                // label3
                // 
                Label time = new Label();
                //date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                time.AutoSize = true;
                time.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
                time.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(159)))));
                time.Location = new System.Drawing.Point(612, 11);
                //date.Name = "label3";
                time.Size = new System.Drawing.Size(71, 29);
                time.TabIndex = 1;
                time.Text = ds.Tables["history"].Rows[i][5].ToString();
                // 
                // panel4
                // 
                Panel topicPanel = new Panel();
                topicPanel.Controls.Add(btnDelete);
                topicPanel.Controls.Add(time);
                topicPanel.Controls.Add(topic);
                topicPanel.Dock = System.Windows.Forms.DockStyle.Top;
                topicPanel.Location = new System.Drawing.Point(0, 0);
                //topicPanel.Name = "panel4";
                topicPanel.Size = new System.Drawing.Size(848, 50);
                topicPanel.TabIndex = 0;
                topicPanel.BorderStyle = BorderStyle.FixedSingle;


                // 
                // label4
                // 
                Label des = new Label();
                des.AllowDrop = true;
                des.AutoSize = false;
                des.Font = new System.Drawing.Font("Noto Serif Lao", 11F);
                //des.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
                des.Location = new System.Drawing.Point(18, 72);
                //des.Name = "label4";
                des.Size = new System.Drawing.Size(900, 39);
                des.TabIndex = 1;
                des.Dock = DockStyle.Fill;
                des.TextAlign = ContentAlignment.MiddleLeft;
                des.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
                des.Text = ds.Tables["history"].Rows[i][3].ToString();
                // 
                // panel2
                // 
                Panel desPanel = new Panel();
                //desPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                desPanel.Controls.Add(des);
                desPanel.Controls.Add(topicPanel);
                desPanel.Location = new System.Drawing.Point(141, 78);
                //desPanel.Name = "panel2";
                desPanel.Size = new System.Drawing.Size(850, 157);
                desPanel.TabIndex = 1;
                desPanel.BackColor = Color.White;
                // 
                // label1
                // 
                Label dateLabel = new Label();
                dateLabel.AutoSize = true;
                dateLabel.BackColor = System.Drawing.Color.FromArgb(1, 127, 119);
                dateLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                dateLabel.Font = new System.Drawing.Font("Mongolian Baiti", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dateLabel.ForeColor = System.Drawing.Color.White;
                dateLabel.Location = new System.Drawing.Point(12, 9);
                //dateLabel.Name = "label1";
                dateLabel.Padding = new System.Windows.Forms.Padding(13, 10, 13, 10);
                dateLabel.Size = new System.Drawing.Size(159, 46);
                dateLabel.TabIndex = 0;
                dateLabel.Text = ds.Tables["history"].Rows[i][4].ToString();

                // 
                // panel3
                // 
                Panel dec = new Panel();
                dec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
                dec.Location = new System.Drawing.Point(84, 37);
                //dec.Name = "panel3";
                dec.Size = new System.Drawing.Size(10, 209);
                dec.TabIndex = 2;
                // 
                // panel1
                // 
                Panel mainPanel = new Panel();
                mainPanel.BackColor = Color.FromArgb(236, 240, 245);
                mainPanel.Controls.Add(desPanel);
                mainPanel.Controls.Add(dateLabel);
                mainPanel.Controls.Add(dec);
                mainPanel.Dock = System.Windows.Forms.DockStyle.Top;
                mainPanel.Location = new System.Drawing.Point(0, 0);
                mainPanel.Name = "mainPanel";
                mainPanel.Size = new System.Drawing.Size(1190, 256);
                mainPanel.TabIndex = 0;

                this.panel2.Controls.Add(mainPanel);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        string patientID = "";
        public void patient(string id)
        {
            patientID = id;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("ກະລຸນາເລືອກການຮັກສາ");
                return;
            }

            sql = "insert into record(patientID, vetID, treatmentID, description, date, time) values (@patientID, @vetID, @noteID, @des, @date, @time)";
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("patientID", patientID);
            cmd.Parameters.AddWithValue("vetID", GetID.staffID);
            cmd.Parameters.AddWithValue("noteID", comboBox1.SelectedValue);
            cmd.Parameters.AddWithValue("des", textBox1.Text);
            cmd.Parameters.AddWithValue("date", System.DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("time", System.DateTime.Now.ToString("hh:mm tt"));
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Save data successfully");
                CreateHistory();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowTreatment();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
