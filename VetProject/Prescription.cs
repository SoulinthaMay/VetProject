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
    public partial class Prescription : Form
    {
        public Prescription()
        {
            InitializeComponent();
        }

        MedicalRecord _m;
        public Prescription(MedicalRecord m)
        {
            InitializeComponent();
            _m = m;
        }

        string patientID = "";
        public void patient(string id)
        {
            patientID = id;
        }

        int i = 0;
        int totalprice = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            totalprice = int.Parse(numericUpDown1.Value.ToString()) * int.Parse(textBox1.Text);
            if (button3.Text == " Add")
            {
                sql = "";
                
                if (dataGridView1.RowCount == 0)
                {
                    i++;
                    dataGridView1.Rows.Add(i, comboBox2.SelectedValue, comboBox2.Text, numericUpDown1.Value, totalprice);
                }
                else if (dataGridView1.RowCount > 0)
                {
                    for (int j = 0; j <= dataGridView1.RowCount - 1; j++)
                    {
                        if (comboBox2.SelectedValue.ToString() == dataGridView1.Rows[j].Cells[1].Value.ToString())
                        {
                            dataGridView1.Rows[j].Cells[3].Value = int.Parse(dataGridView1.Rows[j].Cells[3].Value.ToString()) + int.Parse(numericUpDown1.Value.ToString());
                            dataGridView1.Rows[j].Cells[4].Value = int.Parse(dataGridView1.Rows[j].Cells[4].Value.ToString()) + totalprice;

                            return;
                        }
                    }

                    i++;
                    dataGridView1.Rows.Add(i, comboBox2.SelectedValue, comboBox2.Text, numericUpDown1.Value, totalprice);
                }
            }
            else if (button3.Text == "  Update")
            {
                dataGridView1.Rows[indexrow].Cells[1].Value = comboBox2.SelectedValue;
                dataGridView1.Rows[indexrow].Cells[2].Value = comboBox2.Text;
                dataGridView1.Rows[indexrow].Cells[3].Value = numericUpDown1.Value;
                dataGridView1.Rows[indexrow].Cells[4].Value = totalprice;
            }

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        MySqlConnection conn = DB.getConnect();
        MySqlCommand cmd;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        DataSet ds = new DataSet();
        string sql = "";

        AutoCompleteStringCollection auto = new AutoCompleteStringCollection();

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

        private void fetchPrice()
        {
            sql = "select price from medicine where ID = '" + comboBox2.SelectedValue.ToString() + "'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                textBox1.Text = dr["price"].ToString();
            }
            dr.Close();
        }
        private void fetchMed()
        {
            sql = "SELECT A.ID, concat(A.name, ' (', B.unit, ')') as name FROM `medicine` A inner join unit B on A.unitID = B.unitID order by A.name ASC";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "med");
            if (ds.Tables["med"] != null)
            {
                ds.Tables["med"].Clear();
            }
            da.Fill(ds.Tables["med"]);
            comboBox2.DataSource = ds.Tables["med"];
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "ID";
        }

        string preIDUpdate = "";

        private void Check()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            sql = "select A.ID, B.medID, concat(E.name, ' ', E.surname) as fullname, C.name, B.quantity, B.price from prescription A inner join prescriptdetail B on A.ID = B.preID inner join medicine C on B.medID = C.ID inner join staff E on A.vetID = E.ID where A.status = 'NOT PAID' and A.date = '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' and A.patientID = '" + GetPatientID.patientID + "'";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["medID"].ToString(), dr["name"].ToString(), dr["quantity"].ToString(), dr["price"].ToString());
                    cbVet.Text = dr["fullname"].ToString();
                    preIDUpdate = dr["ID"].ToString();
                }
                button1.Text = "  Update";
            }
            dr.Close();
        }
        private void Prescription_Load(object sender, EventArgs e)
        {
            fetchMed();
            completeData();
            CreateHistory();
            SwitchLanguage.setLanguage();
            fetchPrice();
            fetchVet();
            Check();
        }


        private void completeData()
        {
            sql = "select name from medicine";
            cmd = new MySqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                auto.Add(dr["name"].ToString());
            }
            dr.Close();
            comboBox2.AutoCompleteCustomSource = auto;
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        int indexrow = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexrow = dataGridView1.CurrentRow.Index;
            comboBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox1.Text = int.Parse(dataGridView1.Rows[indexrow].Cells[4].Value.ToString()) / int.Parse(dataGridView1.Rows[indexrow].Cells[3].Value.ToString()) + "";
            numericUpDown1.Value = int.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
            button3.Text = "  Update";

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column6")
            {
                DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                    button3.Text = "Add";
                    numericUpDown1.Value = 1;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Text = "Add";
            numericUpDown1.Value = 1;
        }
        

        string preID = "";
        int quantity = 0;
        int total = 0;

        private void insertupdate()
        {
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("patient", patientID);
            cmd.Parameters.AddWithValue("vet", cbVet.SelectedValue);
            cmd.Parameters.AddWithValue("qty", quantity);
            cmd.Parameters.AddWithValue("total", total);
            cmd.Parameters.AddWithValue("date", System.DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("time", System.DateTime.Now.ToString("hh:mm:ss tt"));
            cmd.Parameters.AddWithValue("status", "NOT PAID");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count  <= 0)
            {
                return;
            }

            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                quantity += int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                total += int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
            }


            ///////////////////////////////////////////
            if (button1.Text == "  Save")
            {
                sql = "insert into prescription(patientID, vetID, qty, total, date, time, status) values (@patient, @vet, @qty, @total, @date, @time, @status)";
                insertupdate();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    sql = "select ID from prescription where patientID = '" + patientID + "' order by ID desc limit 1 ";
                    cmd = new MySqlCommand(sql, conn);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        preID = dr["ID"].ToString();
                    }
                    dr.Close();

                    for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
                    {
                        string sql1 = "insert into prescriptdetail(preID, medID, quantity, price) values (@preID, @med, @qty, @price)";
                        cmd = new MySqlCommand(sql1, conn);
                        cmd.Parameters.AddWithValue("preID", preID);
                        cmd.Parameters.AddWithValue("med", dataGridView1.Rows[i].Cells[1].Value.ToString());
                        cmd.Parameters.AddWithValue("qty", dataGridView1.Rows[i].Cells[3].Value.ToString());
                        cmd.Parameters.AddWithValue("price", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        if (cmd.ExecuteNonQuery() == 1)
                        {

                        }
                    }
                    CreateHistory();
                    Check();
                    MessageBox.Show("Save data successfully");
                    numericUpDown1.Value = 1;
                    button3.Text = " Add";
                }
            }
            ///////////////////////////////////
            //////////////////
            else if (button1.Text == "  Update")
            {
                sql = "update prescription set patientID = @patient, vetID = @vet, qty = @qty, total = @total, date = @date, time = @time, status = @status where ID = '" + preIDUpdate + "'";
                insertupdate();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    sql = "delete from prescriptdetail where preID = '" + preIDUpdate + "'";
                    cmd = new MySqlCommand(sql, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {

                    }
                    for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
                    {
                        string sql1 = "insert into prescriptdetail  (preID, medID, quantity, price) values (@preID, @med, @qty, @price)";
                        cmd = new MySqlCommand(sql1, conn);
                        cmd.Parameters.AddWithValue("preID", preIDUpdate);
                        cmd.Parameters.AddWithValue("med", dataGridView1.Rows[i].Cells[1].Value.ToString());
                        cmd.Parameters.AddWithValue("qty", dataGridView1.Rows[i].Cells[3].Value.ToString());
                        cmd.Parameters.AddWithValue("price", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        if (cmd.ExecuteNonQuery() == 1)
                        {

                        }
                    }
                    Check();
                    CreateHistory();
                    MessageBox.Show("Update data successfully");
                    numericUpDown1.Value = 1;
                    button3.Text = " Add";
                }
            }
            ///////////////////////////////////////
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3.Text = " Add";
            numericUpDown1.Value = 1;
            button1.Text = "  Save";
        }
        string id = "";
        private void CreateHistory()
        {
            flowLayoutPanel1.Controls.Clear();
            sql = "select ID from prescription where patientID = '"+patientID+"' order by ID desc";
            da = new MySqlDataAdapter(sql, conn);
            da.Fill(ds, "1");
            if (ds.Tables["1"] != null)
            {
                ds.Tables["1"].Clear();
            }
            da.Fill(ds, "1");
            for (int k = 0; k <= ds.Tables["1"].Rows.Count - 1; k++)
            {
                sql = "select D.name as ຢາ, E.unit as ຫົວໜ່ວຍ, C.quantity as ຈຳນວນ, DATE_FORMAT(A.date, '%d-%m-%Y') as ວັນທີ, A.status  from prescription A inner join prescriptdetail C on A.ID = C.preID inner join medicine D on C.medID = D.ID inner join unit E on D.unitID = E.unitID  where A.ID = '" + ds.Tables["1"].Rows[k][0]+"'";
                cmd = new MySqlCommand(sql, conn);
                dr = cmd.ExecuteReader();

               

                Panel mainPanel = new Panel();

                
                    Button btnDelete = new Button();
                    btnDelete.BackColor = Color.FromArgb(221, 75, 57);
                    btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
                    btnDelete.FlatAppearance.BorderSize = 0;
                    btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(75)))), ((int)(((byte)(57)))));
                    btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btnDelete.ForeColor = System.Drawing.Color.White;
                    //btnDelete.Name = "button1";
                    btnDelete.Size = new System.Drawing.Size(100, 50);
                    btnDelete.Text = "Delete";
                    btnDelete.UseVisualStyleBackColor = false;
                    btnDelete.TextAlign = ContentAlignment.MiddleCenter;
                    btnDelete.Font = new System.Drawing.Font("Mongolian Baiti", 11F);

                Label lbStatus = new Label();

                //lbStatus.Size = new System.Drawing.Size(100, 50);
                lbStatus.AutoSize = true;
                lbStatus.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
                lbStatus.Location = new System.Drawing.Point(13, 11);
                lbStatus.ForeColor = Color.FromArgb(111, 111, 111);


                Panel pn = new Panel();
                    pn.Dock = System.Windows.Forms.DockStyle.Top;
                    pn.Height = 40;
                    
                    pn.Controls.Add(btnDelete);
                pn.Controls.Add(lbStatus);

                    id = ds.Tables["1"].Rows[k][0].ToString();
                    btnDelete.Click += (sender, e) =>
                    {
                        DialogResult dr = MessageBox.Show("ທ່ານຕ້ອງການລຶບຂໍ້ມູນນີ້ບໍ່?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        if (dr == DialogResult.Yes)
                        {
                            sql = "delete from prescription where ID = '" + id + "'";
                            cmd = new MySqlCommand(sql, conn);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                MessageBox.Show("Delete data successfully");
                                CreateHistory();
                            }
                        }
                    };

                    DataGridView dv = new DataGridView();
                    dv.Dock = DockStyle.Fill;
                    dv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dv.AllowUserToAddRows = false;
                    dv.AllowUserToDeleteRows = false;
                    dv.ReadOnly = true;
                    dv.BackgroundColor = Color.White;
                    dv.GridColor = Color.White;
                    dv.ColumnHeadersHeight = 40;
                    dv.RowTemplate.Height = 43;
                    //dv.DataSource = ds.Tables["history"];
                    dv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(1, 127, 119);
                dv.Columns.Add("Column10", "ຢາ");
                dv.Columns.Add("Column11", "ຫົວໜ່ວຍ");
                dv.Columns.Add("Column12", "ຈຳນວນ");
               
                    // panel2
                    // 
                    Panel desPanel = new Panel();
                    //desPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    desPanel.Controls.Add(dv);
                    desPanel.Controls.Add(pn);
                    desPanel.Location = new System.Drawing.Point(160, 50);
                    //desPanel.Name = "panel2";
                    desPanel.Size = new System.Drawing.Size(920, 220);
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
                    //dateLabel.Text = ds.Tables["history"].Rows[i][3].ToString();

                while (dr.Read())
                {
                    lbStatus.Text = "Status: "+dr["status"].ToString();
                    dv.Rows.Add(dr["ຢາ"].ToString(), dr["ຫົວໜ່ວຍ"].ToString(), dr["ຈຳນວນ"].ToString());
                    dateLabel.Text = dr["ວັນທີ"].ToString();
                }
                dr.Close();

                // 
                // panel3
                // 
                Panel dec = new Panel();
                    dec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
                    dec.Location = new System.Drawing.Point(84, 37);
                    //dec.Name = "panel3";
                    dec.Size = new System.Drawing.Size(10, 230);
                    dec.TabIndex = 2;
                    // 
                    // panel1
                    // 
                    //Panel mainPanel = new Panel();
                    mainPanel.BackColor = Color.FromArgb(236, 240, 245);
                    mainPanel.Controls.Add(desPanel);
                    mainPanel.Controls.Add(dateLabel);
                    mainPanel.Controls.Add(dec);
                    mainPanel.Dock = System.Windows.Forms.DockStyle.Top;
                    mainPanel.Location = new System.Drawing.Point(0, 0);
                    mainPanel.Name = "mainPanel";
                    mainPanel.Size = new System.Drawing.Size(1200, 300);
                    mainPanel.TabIndex = 0;

                

                this.flowLayoutPanel1.Controls.Add(mainPanel);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchPrice();
        }
    }
}
