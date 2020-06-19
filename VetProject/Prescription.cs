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
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Add")
            {
                if (dataGridView1.RowCount == 0)
                {
                    i++;
                    dataGridView1.Rows.Add(i, comboBox2.SelectedValue, comboBox2.Text, comboBox1.SelectedValue, comboBox1.Text, numericUpDown1.Value);

                }
                else if (dataGridView1.RowCount > 0)
                {
                    for (int j = 0; j <= dataGridView1.RowCount - 1; j++)
                    {
                        if (comboBox2.SelectedValue.ToString() == dataGridView1.Rows[j].Cells[1].Value.ToString() && comboBox1.SelectedValue.ToString() == dataGridView1.Rows[j].Cells[3].Value.ToString())
                        {
                            dataGridView1.Rows[j].Cells[5].Value = int.Parse(dataGridView1.Rows[j].Cells[5].Value.ToString()) + int.Parse(numericUpDown1.Value.ToString());

                            return;
                        }
                    }
                    i++;
                    dataGridView1.Rows.Add(i, comboBox2.SelectedValue, comboBox2.Text, comboBox1.SelectedValue, comboBox1.Text, numericUpDown1.Value);

                }
            }
            else if (button3.Text == "Update")
            {
                dataGridView1.Rows[indexrow].Cells[1].Value = comboBox2.SelectedValue;
                dataGridView1.Rows[indexrow].Cells[2].Value = comboBox2.Text;
                dataGridView1.Rows[indexrow].Cells[3].Value = comboBox1.SelectedValue;
                dataGridView1.Rows[indexrow].Cells[4].Value = comboBox1.Text;
                dataGridView1.Rows[indexrow].Cells[5].Value = numericUpDown1.Value;
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

        private void fetchUnit()
        {
            sql = "select * from unit";
            cmd = new MySqlCommand(sql, conn);
            da = new MySqlDataAdapter(cmd);
            da.Fill(ds, "unit");
            if (ds.Tables["unit"] != null)
            {
                ds.Tables["unit"].Clear();
            }
            da.Fill(ds.Tables["unit"]);
            comboBox1.DataSource = ds.Tables["unit"];
            comboBox1.DisplayMember = "unit";
            comboBox1.ValueMember = "unitID";
        }
        private void fetchMed()
        {
            sql = "select * from medicine";
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
        private void Prescription_Load(object sender, EventArgs e)
        {
            fetchUnit();
            fetchMed();
            completeData();
            CreateHistory();
            SwitchLanguage.setLanguage();
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
            comboBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            numericUpDown1.Value = int.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString());
            button3.Text = "Update";

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
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count  <= 0)
            {
                return;
            }

            int quantity = 0;
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                quantity += int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            }

            sql = "insert into prescription(patientID, vetID, qty, date, time) values (@patient, @vet, @qty, @date, @time)";
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("patient", patientID);
            cmd.Parameters.AddWithValue("vet", GetID.staffID);
            cmd.Parameters.AddWithValue("qty", quantity);
            cmd.Parameters.AddWithValue("date", System.DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("time", System.DateTime.Now.ToString("hh:mm:ss tt"));
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
                    string sql1 = "insert into prescriptdetail(preID, medID, unitID, quantity) values (@preID, @med, @unit, @qty)";
                    cmd = new MySqlCommand(sql1, conn);
                    cmd.Parameters.AddWithValue("preID", preID);
                    cmd.Parameters.AddWithValue("med", dataGridView1.Rows[i].Cells[1].Value.ToString());
                    cmd.Parameters.AddWithValue("unit", dataGridView1.Rows[i].Cells[3].Value.ToString());
                    cmd.Parameters.AddWithValue("qty", dataGridView1.Rows[i].Cells[5].Value.ToString());
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        CreateHistory();
                    }
                }
                MessageBox.Show("Save data successfully");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3.Text = "Add";
            numericUpDown1.Value = 1;
            button1.Text = "Save";
        }

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
                sql = "select D.name as ຢາ, E.unit as ຫົວໜ່ວຍ, C.quantity as ຈຳນວນ, DATE_FORMAT(A.date, '%d-%m-%Y') as ວັນທີ from prescription A inner join prescriptdetail C on A.ID = C.preID inner join medicine D on C.medID = D.ID inner join unit E on C.unitID = E.unitID  where A.ID = '"+ds.Tables["1"].Rows[k][0]+"'";
                cmd = new MySqlCommand(sql, conn);
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "history");
                if (ds.Tables["history"] != null)
                {
                    ds.Tables["history"].Clear();
                }
                da.Fill(ds, "history");

                string id = "";

                Panel mainPanel = new Panel();

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
                    dv.DataSource = ds.Tables["history"];
                    dv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(1, 127, 119);
                    // panel2
                    // 
                    Panel desPanel = new Panel();
                    //desPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    desPanel.Controls.Add(dv);
                    desPanel.Controls.Add(btnDelete);
                    desPanel.Location = new System.Drawing.Point(141, 78);
                    //desPanel.Name = "panel2";
                    desPanel.Size = new System.Drawing.Size(920, 180);
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
                    dateLabel.Text = ds.Tables["history"].Rows[i][3].ToString();

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

                }

                this.flowLayoutPanel1.Controls.Add(mainPanel);
            }
        }
    }
}
