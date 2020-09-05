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
            CreateHistory();
            fetchVet();
            Check();
        }

        string recordIDUpdate = "";

        private void Check()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select A.recordID, B.treatmentID, concat(E.name, ' ', E.surname) as fullname, A.description, C.name, D.name as type, B.price from record A inner join recorddetail B on A.recordID = B.recordID inner join treatment C on B.treatmentID = C.ID inner join treatmenttype D on C.typeID = D.ID inner join staff E on A.vetID = E.ID where A.status = 'NOT PAID' and A.date = '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' and A.patientID = '"+ GetPatientID.patientID + "'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["treatmentID"].ToString(), dr["type"].ToString(), dr["name"].ToString(), dr["price"].ToString());
                    cbVet.Text = dr["fullname"].ToString();
                    textBox1.Text = dr["description"].ToString();
                    recordIDUpdate = dr["recordID"].ToString();
                }
                button2.Text = "  Update";
            }
            dr.Close();
        }

        private void fetchVet()
        {
            sql = "select A.ID, concat(A.name, ' ', A.surname) as fullname from staff A inner join job B on A.job = B.jobID where B.jobName = 'ແພດ'";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "vet");
            if (ds.Tables["vet"] != null)
            {
                ds.Tables["vet"].Clear();
            }
            da.Fill(ds.Tables["vet"]);
            cbVet.DataSource = ds.Tables["vet"];
            cbVet.DisplayMember = "fullname";
            cbVet.ValueMember = "ID";
        }

        Color beauty = Color.FromArgb(24, 121, 111);
        private void CreateHistory()
        {
            panel2.Controls.Clear();

            sql = "select recordID from record where patientID = '" + GetPatientID.patientID + "' order by recordID desc";
            da = new MySqlDataAdapter(sql, conn);
            da.Fill(ds, "1");
            if (ds.Tables["1"] != null)
            {
                ds.Tables["1"].Clear();
            }
            da.Fill(ds, "1");


            for (int h = 0; h <= ds.Tables["1"].Rows.Count - 1; h++)
            {
                
                sql = "select A.recordID,A.patientID, A.description, DATE_FORMAT(A.date, '%d-%m-%Y'),A.time from record A inner join recorddetail B on A.recordID = B.recordID where A.recordID = '" + ds.Tables["1"].Rows[h][0] + "'";
                cmd = new MySqlCommand(sql, conn);
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "history");
                if (ds.Tables["history"] != null)
                {
                    ds.Tables["history"].Clear();
                }
                da.Fill(ds, "history");


                sql = "select C.name, B.status from recorddetail A inner join record B on A.recordID = B.recordID inner join treatment C on A.treatmentID = C.ID where A.recordID = '" + ds.Tables["1"].Rows[h][0] + "'";
                cmd = new MySqlCommand(sql, conn);
                dr = cmd.ExecuteReader();

                
                Button btnDelete = new Button();
                    btnDelete.BackColor = Color.FromArgb(221, 75, 57);
                    btnDelete.Dock = DockStyle.Right;
                    btnDelete.FlatAppearance.BorderSize = 0;
                    btnDelete.FlatStyle = FlatStyle.Flat;
                    btnDelete.ForeColor = Color.White;
                    btnDelete.Size = new Size(95, 50);
                    btnDelete.Text = "Delete";
                    btnDelete.TextAlign = ContentAlignment.MiddleCenter;
                    btnDelete.Font = new System.Drawing.Font("Mongolian Baiti", 11F);

                
                Label description = new Label();
                    description.Font = new System.Drawing.Font("Noto Serif Lao", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    description.Location = new System.Drawing.Point(13, 11);
                    description.Size = new System.Drawing.Size(280, 50);
                    description.TabIndex = 0;
                    description.Name = ds.Tables["history"].Rows[0][0].ToString();
                    ////////////////////////////////
                    /////////////////////////////
                    //////////////////////////////////
                    ///
                    btnDelete.Click += (sender, e) =>
                    {
                        DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        if (dr == DialogResult.Yes)
                        {
                            sql = "delete from record where recordID = '" + description.Name + "'";
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
                    time.AutoSize = true;
                    time.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
                    time.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(153)))), ((int)(((byte)(159)))));
                    time.Location = new System.Drawing.Point(610, 15);
                    time.Size = new System.Drawing.Size(71, 29);
                    time.TabIndex = 1;

                    Panel descriptionPanel = new Panel();
                descriptionPanel.Controls.Add(btnDelete);
                
                descriptionPanel.Controls.Add(time);
                    descriptionPanel.Controls.Add(description);
                    descriptionPanel.Dock = System.Windows.Forms.DockStyle.Top;
                    descriptionPanel.Location = new System.Drawing.Point(0, 0);
                    //descriptionPanel.Name = "panel4";
                    descriptionPanel.Size = new System.Drawing.Size(848, 60);
                    descriptionPanel.TabIndex = 0;
                    descriptionPanel.BorderStyle = BorderStyle.FixedSingle;
                
                DataGridView dgTreatment = new DataGridView();
                    dgTreatment.Font = new System.Drawing.Font("Noto Serif Lao", 11F);
                    dgTreatment.Dock = DockStyle.Fill;
                dgTreatment.ReadOnly = true;
                    dgTreatment.RowTemplate.Height = 43;
                    dgTreatment.ColumnHeadersVisible = false;
                    dgTreatment.GridColor = Color.White;
                    dgTreatment.BackgroundColor = Color.White;
                    dgTreatment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgTreatment.AllowUserToAddRows = false;
                    dgTreatment.AllowUserToDeleteRows = false;
                    dgTreatment.DefaultCellStyle.SelectionBackColor = Color.FromArgb(1, 127, 119);
                    dgTreatment.DefaultCellStyle.SelectionForeColor = Color.White;

                dgTreatment.Columns.Add("Column9", "ລຳດັບ");
                dgTreatment.Columns.Add("Column10", "ການຮັກສາ");
                dgTreatment.Columns.Add("Column11", "ສະຖານະ");

                dgTreatment.Columns[0].FillWeight = 20;
                dgTreatment.Columns[2].FillWeight = 40;
                int i = 0;
                while (dr.Read())
                {
                    i++;
                    dgTreatment.Rows.Add(i, dr["name"].ToString(), dr["status"].ToString());
                }
                dr.Close();
                    // 
                    // panel2
                    // 
                    Panel desPanel = new Panel();
                    //desPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    desPanel.Controls.Add(dgTreatment);
                    desPanel.Controls.Add(descriptionPanel);
                    desPanel.Location = new System.Drawing.Point(141, 78);
                    //desPanel.Name = "panel2";
                    desPanel.Size = new System.Drawing.Size(850, 170);
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

                description.Text = ds.Tables["history"].Rows[0][2].ToString();
                dateLabel.Text = ds.Tables["history"].Rows[0][3].ToString();
                time.Text = ds.Tables["history"].Rows[0][4].ToString();

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
            addTreatment a = new addTreatment(this);
            a.ShowDialog();
        }

        public void fetchData(string id, string type, string treatment, string price)
        {
            int i = dataGridView1.Rows.Count;
            if (dataGridView1.RowCount == 0)
            {
                i++;
                dataGridView1.Rows.Add(i, id, type, treatment, price);
            }
            else if (dataGridView1.RowCount > 0)
            {
                for (int j = 0; j <= dataGridView1.RowCount - 1; j++)
                {
                    if (id == dataGridView1.Rows[j].Cells[1].Value.ToString())
                    {
                        MessageBox.Show("ຂໍ້ມູນຊ້ຳກັນ");
                        return;
                    }
                }
                i++;
                dataGridView1.Rows.Add(i, id, type, treatment, price);
            }
        }
        
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        int total = 0;
        private void insertupdate()
        {
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("patientID", GetPatientID.patientID);
            cmd.Parameters.AddWithValue("vetID", cbVet.SelectedValue);
            cmd.Parameters.AddWithValue("description", textBox1.Text);
            cmd.Parameters.AddWithValue("total", total);
            cmd.Parameters.AddWithValue("date", System.DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("time", System.DateTime.Now.ToString("HH:mm:ss"));
            cmd.Parameters.AddWithValue("status", "NOT PAID");
        }

        string recordID = "";
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("ບໍ່ມີຂໍ້ມູນ");
                return;
            }

            for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            {
                total += int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
            }

            if (button2.Text == "  Save")
            {
                sql = "insert into record (patientID, vetID, description, total, date, time, status) values (@patientID, @vetID, @description, @total, @date, @time, @status)";

                insertupdate();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    sql = "select recordID from record order by recordID DESC limit 1";
                    cmd = new MySqlCommand(sql, conn);

                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        recordID = dr["recordID"].ToString();
                    }
                    dr.Close();
                    for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                    {
                        string sql1 = "insert into recorddetail (recordID, treatmentID, price) values (@recordID, @treatmentID, @price)";
                        cmd = new MySqlCommand(sql1, conn);
                        cmd.Parameters.AddWithValue("recordID", recordID);
                        cmd.Parameters.AddWithValue("treatmentID", dataGridView1.Rows[i].Cells[1].Value.ToString());
                        cmd.Parameters.AddWithValue("price", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            
                        }
                    }
                    Check();
                    MessageBox.Show("Save data successfully");
                    CreateHistory();

                }

            }
            else if (button2.Text == "  Update")
            {
                    sql = "update record set patientID = @patientID, vetID = @vetID, description = @description, total = @total, date = @date, time = @time, status = @status where recordID = '" + recordIDUpdate + "'";

                    insertupdate();
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                    sql = "delete from recorddetail where recordID = '" + recordIDUpdate + "'";
                    cmd = new MySqlCommand(sql, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {

                    }
                        for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                        {
                        //string sql1 = "update recorddetail set treatmentID = @treatmentID, price = @price where recordID = @recordID";
                        string sql1 = "insert into recorddetail (recordID, treatmentID, price) values (@recordID, @treatmentID, @price)";
                            cmd = new MySqlCommand(sql1, conn);
                            cmd.Parameters.AddWithValue("recordID", recordIDUpdate);
                            cmd.Parameters.AddWithValue("treatmentID", dataGridView1.Rows[i].Cells[1].Value.ToString());
                            cmd.Parameters.AddWithValue("price", dataGridView1.Rows[i].Cells[4].Value.ToString());
                            if (cmd.ExecuteNonQuery() == 1)
                            {

                            }
                        }
                        Check();
                        MessageBox.Show("Update data successfully");
                        CreateHistory();
                    }

               
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            dataGridView1.Rows.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column6")
            {
                DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                }
            }
        }
    }
}
