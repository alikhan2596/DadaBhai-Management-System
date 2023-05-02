using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace MIS_Project
{
    public partial class Form1 : Form
    {
        private FormWindowState windowState;
        
        public static string con_str = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Ali Khan\Documents\misProj.mdb;";
        OleDbConnection conn = new OleDbConnection(con_str);

        public FormWindowState WindowState1 { get => windowState; set => windowState = value; }
        public int Sales { get; private set; }

        public Form1()
        {
            Thread t = new Thread(new ThreadStart(SplashStart));
            t.Start();
            Thread.Sleep(5000);
            InitializeComponent();
            t.Abort();
        }

        //SplashScreen
        public void SplashStart()
        {
            Application.Run(new Form_SplashScreen());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DataSet1.DataTable2' table. You can move, or remove it, as needed.
            this.dataTable2TableAdapter.Fill(this.DataSet1.DataTable2);
            // TODO: This line of code loads data into the 'misProjDataSet.employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.misProjDataSet.employee);
            // TODO: This line of code loads data into the 'misProjDataSet.product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.misProjDataSet.product);

            comboBox_jobtype.Items.Add("Employee");
            comboBox_jobtype.Items.Add("Admin");
            panel_login.Visible = true;
            panel_mainPanel.Visible = true;

            //this.reportViewer_sales.RefreshReport();
            //this.reportViewer1.RefreshReport();
            conn.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = conn;
            string query = "select p_name from product;";
            cmd.CommandText = query;
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox_selectProduct1.Items.Add(reader["P_name"].ToString());
                comboBox_selectProduct2.Items.Add(reader["P_name"].ToString());
                comboBox_selectProduct3.Items.Add(reader["P_name"].ToString());
                comboBox_selectProduct4.Items.Add(reader["P_name"].ToString());
            }
            conn.Close();

            string query0 = "select max(o_id) from [order]";

            int oid ;
            conn.Open();
            //OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = query0;
            cmd.Connection = conn;
            //OleDbDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    oid = reader["o_id"].ToString();
            //}
            oid = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            //MessageBox.Show(oid);
            conn.Close();
            label9.Text = oid.ToString();



            conn.Open();
            //OleDbCommand cmd = new OleDbCommand();
            string query11 = "SELECT product.P_id, product.P_name, product.P_unitPrice, product.initQuantity, order_Product.quantitySold FROM(order_Product RIGHT OUTER JOIN product ON order_Product.P_Id = product.P_id)";
            cmd.CommandText = query11;
            cmd.Connection = conn;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView_inventory.DataSource = dt;
            OleDbCommandBuilder cmdb = new OleDbCommandBuilder(da);
            da.Update(dt);

            conn.Close();
            //this.reportViewer1.RefreshReport();
            //this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();
            this.reportViewer3.RefreshReport();
        }


        //LoginForm

        private void txt_loginuser_Enter(object sender, EventArgs e)
        {
            if (txt_loginuser.Text == "Username")
            {
                txt_loginuser.Text = "";
                txt_loginuser.ForeColor = Color.Black;
            }
        }

        private void txt_loginuser_Leave(object sender, EventArgs e)
        {
            if (txt_loginuser.Text == "")
            {
                txt_loginuser.Text = "Username";
                txt_loginuser.ForeColor = Color.DarkGray;
            }
        }

        private void txt_loginpass_Enter(object sender, EventArgs e)
        {
            if (txt_loginpass.Text == "Password")
            {
                txt_loginpass.UseSystemPasswordChar = true;
                txt_loginpass.Text = "";
                txt_loginpass.ForeColor = Color.Black;
            }
        }

        private void txt_loginpass_Leave(object sender, EventArgs e)
        {
            if (txt_loginpass.Text == "")
            {
                txt_loginpass.UseSystemPasswordChar = false;
                txt_loginpass.Text = "Password";
                txt_loginpass.ForeColor = Color.DarkGray;
            }
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            panel_login.Visible = false;
            string cst = "select count(*) from Admin where email = '" + txt_loginuser.Text + "' and password='" + txt_loginpass.Text + "'";
            string emp = "select count(*) from employee where email = '" + txt_loginuser.Text + "' and password='" + txt_loginpass.Text + "'";
            //string empName = "select fname " + " la";
            conn.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = cst;            
            cmd.Connection = conn;



            if ((int)cmd.ExecuteScalar() == 1)
            {
                panel_head.Visible = true;
                panel_AdminMain.Visible = true;
                panel_admin.Visible = true;
                panel_AdminSide.Visible = true;

            }

            else
            {
            cmd.CommandText = emp;
                cmd.Connection = conn;
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    panel_head.Visible = true;
                    panel_employeeSide.Visible = true;
                    panel_EmpMain.Visible = true;
                    panel_order1.Visible = true;
                    
                }
            }
            conn.Close();
        }

        private void link_signup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel_signup.Visible = true;
        }

        //Signup Form

        private void txt_signfname_Enter(object sender, EventArgs e)
        {
            if (txt_signfname.Text == "First Name")
            {
                txt_signfname.Text = "";
                txt_signfname.ForeColor = Color.Black;

            }
        }

        private void txt_signfname_Leave(object sender, EventArgs e)
        {
            if (txt_signfname.Text == "")
            {
                txt_signfname.Text = "First Name";
                txt_signfname.ForeColor = Color.DarkGray;

            }
        }

        private void txt_signlname_Enter(object sender, EventArgs e)
        {
            if (txt_signlname.Text == "Last Name")
            {
                txt_signlname.Text = "";
                txt_signlname.ForeColor = Color.Black;

            }
        }

        private void txt_signlname_Leave(object sender, EventArgs e)
        {
            if (txt_signlname.Text == "")
            {
                txt_signlname.Text = "Last Name";
                txt_signlname.ForeColor = Color.DarkGray;
            }
        }

        private void txt_signemail_Enter(object sender, EventArgs e)
        {
            if (txt_signemail.Text == "email")
            {
                txt_signemail.Text = "";
                txt_signemail.ForeColor = Color.Black;
            }
        }

        private void txt_signemail_Leave(object sender, EventArgs e)
        {
            if (txt_signemail.Text == "")
            {
                txt_signemail.Text = "email";
                txt_signemail.ForeColor = Color.DarkGray;
            }
        }

        private void txt_signpass_Enter(object sender, EventArgs e)
        {
            if (txt_signpass.Text == "Password")
            {
                txt_signpass.UseSystemPasswordChar = true;
                txt_signpass.Text = "";
                txt_signpass.ForeColor = Color.Black;
            }
        }

        private void txt_signpass_Leave(object sender, EventArgs e)
        {
            if (txt_signpass.Text == "")
            {
                txt_signpass.UseSystemPasswordChar = false;
                txt_signpass.Text = "Password";
                txt_signpass.ForeColor = Color.DarkGray;
            }
        }

        private void txt_signphone_Enter(object sender, EventArgs e)
        {
            if (txt_signphone.Text == "Phone No")
            {
                txt_signphone.Text = "";
                txt_signphone.ForeColor = Color.Black;
            }
        }

        private void txt_signphone_Leave(object sender, EventArgs e)
        {
            if (txt_signphone.Text == "")
            {
                txt_signphone.Text = "Phone No";
                txt_signphone.ForeColor = Color.DarkGray;
            }
        }

        private void comboBox_jobtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }
        private void txt_signadd_Enter(object sender, EventArgs e)
        {
            if (txt_signadd.Text == "Address")
            {
                txt_signadd.Text = "";
                txt_signadd.ForeColor = Color.Black;
            }
        }

        private void txt_signadd_Leave(object sender, EventArgs e)
        {
            if (txt_signadd.Text == "")
            {
                txt_signadd.Text = "Address";
                txt_signadd.ForeColor = Color.DarkGray;
            }
        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            //
            conn.Open();
            OleDbCommand cmd = new OleDbCommand();
            string table;
            if (comboBox_jobtype.Text == "Admin")            
                table = "Admin";            
            else
                table = "employee";

            string query = "insert into " + table + " ([fname],[lname],[email],[password],[phone],[address]) values ('" + txt_signfname.Text + "','" + txt_signlname.Text + "','" + txt_signemail.Text + "','" + txt_signpass.Text + "','" + txt_signphone.Text + "','" + txt_signadd.Text + "');";
            cmd.CommandText = query;
            cmd.Connection = conn;
            MessageBox.Show(query);
            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Data inserted!");
                panel_signup.Visible = false;
                panel_login.Visible = true;
            }

            conn.Close();
        }
        private void panel_login_Paint(object sender, PaintEventArgs e)
        {

        }
        

        private void label_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label_minimize_Click(object sender, EventArgs e)
        {

            this.windowState = FormWindowState.Minimized;
        }
        private void link_login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel_signup.Visible = false;
            panel_login.Visible = true;
        }

        private void panel_AdminSide_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chart_sales_Click(object sender, EventArgs e)
        {

        }

        private void btn_adminInfo_Click(object sender, EventArgs e)
        {
            this.chart_sales.Series[Sales].Points.AddXY("mon","pro");
            panel_manageEmp.Visible = false;
            panel_inventory.Visible = false;
            panel_head.Visible = true;
            panel_AdminSide.Visible = true;
            panel_admin.Visible = true;
            panel_Report.Visible = false;
            panel_adminProfile.Visible = false;
        }

        private void btn_adminInventory_Click(object sender, EventArgs e)
        {
            panel_admin.Visible = false;
            panel_manageEmp.Visible = false;
            panel_Report.Visible = false;
            panel_head.Visible = true;
            panel_AdminSide.Visible = true;
            panel_inventory.Visible = true;
            panel_adminProfile.Visible = false;
        }

        private void btn_adminReport_Click(object sender, EventArgs e)
        {
            panel_admin.Visible = false;
            panel_manageEmp.Visible = false;
            panel_inventory.Visible = false;
            panel_Report.Visible = true;
            panel_adminProfile.Visible = false;
            this.order_ProductTableAdapter.Fill(this.misProjDataSet.order_Product);
            reportViewer2.RefreshReport();
        }

        private void btn_adminManage_Click(object sender, EventArgs e)
        {
            panel_admin.Visible = false;
            panel_inventory.Visible = false;
            panel_Report.Visible = false;
            panel_adminProfile.Visible = false;
            panel_manageEmp.Visible = true;
            /*conn.Open();
            OleDbCommand cmd = new OleDbCommand();
            //string query11 = "SELECT product.P_id, product.P_name, product.P_unitPrice, product.initQuantity, order_Product.quantitySold FROM(order_Product RIGHT OUTER JOIN product ON order_Product.P_Id = product.P_id)";
            //cmd.CommandText = query11;
            //cmd.Connection = conn;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            //da.Fill(dt);
            dataGridView_Emp.DataSource = dt;
            OleDbCommandBuilder cmdb = new OleDbCommandBuilder(da);
            da.Update(dt);

            conn.Close();*/
        }

        private void txt_signlname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_signphone_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_inventoryAdd_Click(object sender, EventArgs e)
        {

        }

        private void btn_inventoryUpdate_Click(object sender, EventArgs e)
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand();
            string query11 = "SELECT product.P_id, product.P_name, product.P_unitPrice, product.initQuantity, order_Product.quantitySold FROM(order_Product RIGHT OUTER JOIN product ON order_Product.P_Id = product.P_id)";
            cmd.CommandText = query11;
            cmd.Connection = conn;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            //da.Fill(dt);
            //dataGridView_inventory.DataSource = dt;
            OleDbCommandBuilder cmdb = new OleDbCommandBuilder(da);
            da.Update(dt);

            conn.Close();
        }

        private void btn_inventoryRemove_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel_mainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_SignOut_Click(object sender, EventArgs e)
        {
            panel_AdminMain.Visible = false;
            panel_EmpMain.Visible = false;
            panel_login.Visible = true;
            txt_loginpass.Text = "";
        }

        private void dataGridView_inventory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox_IntProCode.Text = dataGridView_inventory.SelectedRows[0].Cells[0].Value.ToString();
            textBox_IntQuantity.Text = dataGridView_inventory.SelectedRows[0].Cells[1].Value.ToString();
            //dateTimePicker_inventory.Text = dateTimePicker_inventory.Select
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void rectangleShape33_Click(object sender, EventArgs e)
        {

        }

        private void btn_cusOrder_Click(object sender, EventArgs e)
        {
            panel_order1.Visible = true;
            panel_order2.Visible = false;
            panel_order3.Visible = false;
            panel_product.Visible = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel_order2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox2.Visible = true;
            linkLabel1.Visible = false;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btn_CusInfoNext_Click(object sender, EventArgs e)
        {
            panel_order2.Visible = true;
        }

        private void btn_CusOrderBack_Click(object sender, EventArgs e)
        {
            panel_order2.Visible = false;
        }

        private void btn_CusOrderNext_Click(object sender, EventArgs e)
        {
            panel_order3.Visible = true;
        }

        private void btn_CusInvoiceBack_Click(object sender, EventArgs e)
        {
            panel_order3.Visible = false;
        }

        private void btn_CusInvoiceDone_Click(object sender, EventArgs e)
        {
          

            string query = "insert into customer ([fname],[lname],[email],[phone],[address],[area],[city]) values ('"
                + txt_Cusfname.Text + "','" + txt_Cuslname.Text + "','" + txt_CusEmail.Text + "','" + txt_CusPhoneNo.Text
                + "','" + txt_CusStreetAdd.Text + "','" + txt_CusArea.Text + "','" + txt_CusCity.Text + "');";

            conn.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = query;
            cmd.Connection = conn;
            MessageBox.Show(query);
            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Data inserted!");
            }
            conn.Close();


            string query1 = "select p_id from product where p_name='" + comboBox_selectProduct1.Text + "'";
            string query2 = "select e_id from employee where email='" + txt_loginuser.Text + "'";
            string query3 = "select c_id from customer where email='" + txt_CusEmail.Text + "'";


            string eid="";
            conn.Open();
            
            cmd.CommandText = query2;
            cmd.Connection = conn;
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                eid = reader["e_id"].ToString();
            }
            MessageBox.Show("eid="+eid);
            conn.Close();


            string pid = "";
            conn.Open();
            //OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = query1;
            cmd.Connection = conn;
            OleDbDataReader reader1 = cmd.ExecuteReader();
            while (reader1.Read())
            {
                pid = reader1["p_id"].ToString();
            }
            MessageBox.Show("pid = "+pid);
            conn.Close();


            string cid = "";
            conn.Open();
            //OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = query3;
            cmd.Connection = conn;
            OleDbDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                cid = reader2["c_id"].ToString();
            }
            MessageBox.Show("cid = " + cid);
            conn.Close();


            string date1 = System.DateTime.Today.ToString("dd-MMM-yy");
            string query4 = "insert into [order] ([C_id],[E_id],[O_orderDate]) values ('" + cid + "','" + eid + "','" + date1+ "')";

            conn.Open();
            cmd.CommandText = query4;
            cmd.Connection = conn;
            MessageBox.Show(query4);
            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Data inserted!");
            }
            conn.Close();

            string query5 = "insert into order_product([p_id],[o_id],[quantitySold]) values('"+pid+"','"+ label9.Text + "','"+txt_productQuantity1.Text+"')";

            conn.Open();
            cmd.CommandText = query5;
            cmd.Connection = conn;
            MessageBox.Show(query5);
            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Data inserted!");
            }
            conn.Close();

            panel_order3.Visible = true;

        }

        private void txt_Cusfname_Enter(object sender, EventArgs e)
        {
            if (txt_Cusfname.Text == "First Name")
            {
                txt_Cusfname.Text = "";
                txt_Cusfname.ForeColor = Color.Black;
            }
        }

        private void txt_Cusfname_Leave(object sender, EventArgs e)
        {
            if (txt_Cusfname.Text == "")
            {
                txt_Cusfname.Text = "First Name";
                txt_Cusfname.ForeColor = Color.DarkGray;
            }
        }

        private void txt_Cuslname_Enter(object sender, EventArgs e)
        {
            if (txt_Cuslname.Text == "Last Name")
            {
                txt_Cuslname.Text = "";
                txt_Cuslname.ForeColor = Color.Black;
            }
        }

        private void txt_Cuslname_Leave(object sender, EventArgs e)
        {
            if (txt_Cuslname.Text == "")
            {
                txt_Cuslname.Text = "Last Name";
                txt_Cuslname.ForeColor = Color.DarkGray;
            }
        }

        private void txt_CusEmail_Enter(object sender, EventArgs e)
        {
            if (txt_CusEmail.Text == "Email")
            {
                txt_CusEmail.Text = "";
                txt_CusEmail.ForeColor = Color.Black;
            }
        }

        private void txt_CusEmail_Leave(object sender, EventArgs e)
        {
            if (txt_CusEmail.Text == "")
            {
                txt_CusEmail.Text = "Email";
                txt_CusEmail.ForeColor = Color.DarkGray;
            }
        }

        private void txt_CusStreetAdd_Enter(object sender, EventArgs e)
        {
            if (txt_CusStreetAdd.Text == "Street Address")
            {
                txt_CusStreetAdd.Text = "";
                txt_CusStreetAdd.ForeColor = Color.Black;
            }
        }

        private void txt_CusStreetAdd_Leave(object sender, EventArgs e)
        {
            if (txt_CusStreetAdd.Text == "")
            {
                txt_CusStreetAdd.Text = "Street Address";
                txt_CusStreetAdd.ForeColor = Color.DarkGray;
            }
        }

        private void txt_CusArea_Enter(object sender, EventArgs e)
        {
            if (txt_CusArea.Text == "Area")
            {
                txt_CusArea.Text = "";
                txt_CusArea.ForeColor = Color.Black;
            }
        }

        private void txt_CusArea_Leave(object sender, EventArgs e)
        {
            if (txt_CusArea.Text == "")
            {
                txt_CusArea.Text = "Area";
                txt_CusArea.ForeColor = Color.DarkGray;
            }
        }

        private void txt_CusCity_Enter(object sender, EventArgs e)
        {
            if (txt_CusCity.Text == "City")
            {
                txt_CusCity.Text = "";
                txt_CusCity.ForeColor = Color.Black;
            }
        }

        private void txt_CusCity_Leave(object sender, EventArgs e)
        {
            if (txt_CusCity.Text == "")
            {
                txt_CusCity.Text = "City";
                txt_CusCity.ForeColor = Color.DarkGray;
            }
        }

        private void txt_CusPhoneNo_Enter(object sender, EventArgs e)
        {
            if (txt_CusPhoneNo.Text == "Phone No")
            {
                txt_CusPhoneNo.Text = "";
                txt_CusPhoneNo.ForeColor = Color.Black;
            }
        }

        private void txt_CusPhoneNo_Leave(object sender, EventArgs e)
        {
            if (txt_CusPhoneNo.Text == "")
            {
                txt_CusPhoneNo.Text = "Phone No";
                txt_CusPhoneNo.ForeColor = Color.DarkGray;
            }
        }

        private void txt_productQuantity1_Enter(object sender, EventArgs e)
        {
            if (txt_productQuantity1.Text == "Quantity")
            {
                txt_productQuantity1.Text = "";
                txt_productQuantity1.ForeColor = Color.Black;
            }
        }

        private void txt_productQuantity1_Leave(object sender, EventArgs e)
        {
            if (txt_productQuantity1.Text == "")
            {
                txt_productQuantity1.Text = "Quantity";
                txt_productQuantity1.ForeColor = Color.DarkGray;
            }
        }

        private void txt_productQuantity2_Enter(object sender, EventArgs e)
        {
            if (txt_productQuantity2.Text == "Quantity")
            {
                txt_productQuantity2.Text = "";
                txt_productQuantity2.ForeColor = Color.Black;
            }
        }

        private void txt_productQuantity2_Leave(object sender, EventArgs e)
        {
            if (txt_productQuantity2.Text == "")
            {
                txt_productQuantity2.Text = "Quantity";
                txt_productQuantity2.ForeColor = Color.DarkGray;
            }
        }

        private void txt_productQuantity3_Enter(object sender, EventArgs e)
        {
            if (txt_productQuantity3.Text == "Quantity")
            {
                txt_productQuantity3.Text = "";
                txt_productQuantity3.ForeColor = Color.Black;
            }
        }

        private void txt_productQuantity3_Leave(object sender, EventArgs e)
        {
            if (txt_productQuantity3.Text == "")
            {
                txt_productQuantity3.Text = "Quantity";
                txt_productQuantity3.ForeColor = Color.DarkGray;
            }
        }

        private void txt_productQuantity4_Enter(object sender, EventArgs e)
        {
            if (txt_productQuantity4.Text == "Quantity")
            {
                txt_productQuantity4.Text = "";
                txt_productQuantity4.ForeColor = Color.Black;
            }
        }

        private void txt_productQuantity4_Leave(object sender, EventArgs e)
        {
            if (txt_productQuantity4.Text == "")
            {
                txt_productQuantity4.Text = "Quantity";
                txt_productQuantity4.ForeColor = Color.DarkGray;
            }
        }

        private void btn_empAdd_Click(object sender, EventArgs e)
        {

        }

        private void btn_empUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btn_empDelete_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_inventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox3.Visible = true;
            linkLabel2.Visible = false;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox4.Visible = true;
            linkLabel3.Visible = false;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void btn_EmpProduct_Click(object sender, EventArgs e)
        {
            panel_order1.Visible = false;
            panel_product.Visible = true;
            panel_EmpProfile.Visible = false;
        }

        private void btn_EmpProfile_Click(object sender, EventArgs e)
        {
            panel_order1.Visible = false;
            panel_product.Visible = false;
            panel_EmpProfile.Visible = true;
        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.employeeTableAdapter.FillBy(this.misProjDataSet.employee);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void panel_EmpProfile_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_empfname_MouseHover(object sender, EventArgs e)
        {
            rectangleShape47.BackColor = Color.SteelBlue;
            txt_empfname.BackColor = Color.SteelBlue;
            txt_empfname.ForeColor = Color.White;
        }

      
        private void txt_empfname_MouseLeave(object sender, EventArgs e)
        {
            txt_empfname.BackColor = Color.White;
            rectangleShape47.BackColor = Color.Transparent;
            txt_empfname.ForeColor = Color.Black;
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            rectangleShape48.BackColor = Color.SteelBlue;
            txt_emplname.BackColor = Color.SteelBlue;
            txt_emplname.ForeColor = Color.White;
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            txt_emplname.BackColor = Color.White;
            rectangleShape48.BackColor = Color.Transparent;
            txt_emplname.ForeColor = Color.Black;
        }

        private void txt_empEmail_MouseHover(object sender, EventArgs e)
        {
            rectangleShape51.BackColor = Color.SteelBlue;
            txt_empEmail.BackColor = Color.SteelBlue;
            txt_empEmail.ForeColor = Color.White;
        }

        private void txt_empEmail_MouseLeave(object sender, EventArgs e)
        {
            txt_empEmail.BackColor = Color.White;
            rectangleShape51.BackColor = Color.Transparent;
            txt_empEmail.ForeColor = Color.Black;
        }

        private void txt_empPass_MouseHover(object sender, EventArgs e)
        {
            rectangleShape52.BackColor = Color.SteelBlue;
            txt_empPass.BackColor = Color.SteelBlue;
            txt_empPass.ForeColor = Color.White;
        }

        private void txt_empPass_MouseLeave(object sender, EventArgs e)
        {
            txt_empPass.BackColor = Color.White;
            rectangleShape52.BackColor = Color.Transparent;
            txt_empPass.ForeColor = Color.Black;
        }

        private void txt_empAddess_MouseHover(object sender, EventArgs e)
        {
            rectangleShape53.BackColor = Color.SteelBlue;
            txt_empAddess.BackColor = Color.SteelBlue;
            txt_empAddess.ForeColor = Color.White;
        }

        private void txt_empAddess_MouseLeave(object sender, EventArgs e)
        {
            txt_empAddess.BackColor = Color.White;
            rectangleShape53.BackColor = Color.Transparent;
            txt_empAddess.ForeColor = Color.Black;
        }

        private void txt_empPhone_MouseHover(object sender, EventArgs e)
        {
            rectangleShape54.BackColor = Color.SteelBlue;
            txt_empPhone.BackColor = Color.SteelBlue;
            txt_empPhone.ForeColor = Color.White;
        }

        private void txt_empPhone_MouseLeave(object sender, EventArgs e)
        {
            txt_empPhone.BackColor = Color.White;
            rectangleShape54.BackColor = Color.Transparent;
            txt_empPhone.ForeColor = Color.Black;
        }

        private void txt_empSal_MouseHover(object sender, EventArgs e)
        {
            rectangleShape58.BackColor = Color.SteelBlue;
            txt_empSal.BackColor = Color.SteelBlue;
            txt_empSal.ForeColor = Color.White;
        }

        private void txt_empSal_MouseLeave(object sender, EventArgs e)
        {
            txt_empSal.BackColor = Color.White;
            rectangleShape58.BackColor = Color.Transparent;
            txt_empSal.ForeColor = Color.Black;
        }

        private void txt_empComm_MouseHover(object sender, EventArgs e)
        {
            rectangleShape57.BackColor = Color.SteelBlue;
            txt_empComm.BackColor = Color.SteelBlue;
            txt_empComm.ForeColor = Color.White;
        }

        private void txt_empComm_MouseLeave(object sender, EventArgs e)
        {
            txt_empComm.BackColor = Color.White;
            rectangleShape57.BackColor = Color.Transparent;
            txt_empComm.ForeColor = Color.Black;
        }

        private void btn_empProfileSave_Click(object sender, EventArgs e)
        {

        }

        private void panel_product_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                    }

        private void button1_Click(object sender, EventArgs e)
        {
            panel_admin.Visible = false;
            panel_inventory.Visible = false;
            panel_Report.Visible = false;
            panel_manageEmp.Visible = false;
            panel_adminProfile.Visible = true;
        }

        private void txt_adminfname_MouseHover(object sender, EventArgs e)
        {
            rectangleShape68.BackColor = Color.SteelBlue;
            txt_adminfname.BackColor = Color.SteelBlue;
            txt_adminfname.ForeColor = Color.White;
        }

        private void txt_adminfname_MouseLeave(object sender, EventArgs e)
        {
            txt_adminfname.BackColor = Color.White;
            rectangleShape68.BackColor = Color.Transparent;
            txt_adminfname.ForeColor = Color.Black;
        }

        private void txt_adminlname_MouseHover(object sender, EventArgs e)
        {
            rectangleShape67.BackColor = Color.SteelBlue;
            txt_adminlname.BackColor = Color.SteelBlue;
            txt_adminlname.ForeColor = Color.White;
        }

        private void txt_adminlname_MouseLeave(object sender, EventArgs e)
        {
            txt_adminlname.BackColor = Color.White;
            rectangleShape67.BackColor = Color.Transparent;
            txt_adminlname.ForeColor = Color.Black;
        }

        private void txt_adminEmail_MouseHover(object sender, EventArgs e)
        {
            rectangleShape66.BackColor = Color.SteelBlue;
            txt_adminEmail.BackColor = Color.SteelBlue;
            txt_adminEmail.ForeColor = Color.White;
        }

        private void txt_adminEmail_MouseLeave(object sender, EventArgs e)
        {
            txt_adminEmail.BackColor = Color.White;
            rectangleShape66.BackColor = Color.Transparent;
            txt_adminEmail.ForeColor = Color.Black;
        }

        private void txt_adminPass_MouseHover(object sender, EventArgs e)
        {
            rectangleShape65.BackColor = Color.SteelBlue;
            txt_adminPass.BackColor = Color.SteelBlue;
            txt_adminPass.ForeColor = Color.White;
        }

        private void txt_adminPass_MouseLeave(object sender, EventArgs e)
        {
            txt_adminPass.BackColor = Color.White;
            rectangleShape65.BackColor = Color.Transparent;
            txt_adminPass.ForeColor = Color.Black;
        }

        private void txt_adminAdd_MouseHover(object sender, EventArgs e)
        {
            rectangleShape64.BackColor = Color.SteelBlue;
            txt_adminAdd.BackColor = Color.SteelBlue;
            txt_adminAdd.ForeColor = Color.White;
        }

        private void txt_adminAdd_MouseLeave(object sender, EventArgs e)
        {
            txt_adminAdd.BackColor = Color.White;
            rectangleShape64.BackColor = Color.Transparent;
            txt_adminAdd.ForeColor = Color.Black;
        }

        private void txt_adminPhone_MouseHover(object sender, EventArgs e)
        {
            rectangleShape63.BackColor = Color.SteelBlue;
            txt_adminPhone.BackColor = Color.SteelBlue;
            txt_adminPhone.ForeColor = Color.White;
        }

        private void txt_adminPhone_MouseLeave(object sender, EventArgs e)
        {
            txt_adminPhone.BackColor = Color.White;
            rectangleShape63.BackColor = Color.Transparent;
            txt_adminPhone.ForeColor = Color.Black;
        }
    }
}
