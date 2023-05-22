using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace INVENTORY
{
    public partial class Frm_Admin_records : Form
    {
        public Frm_Admin_records()
        {
            InitializeComponent();

            empLIST();
            totalEMP();
            totalUSER();
        }
        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();
        SqlDataAdapter adapt;
        DataTable dt;


        // DATA GRIDVIEW ----------------------------------------------------------------------------------start
        public void empLIST()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = @"SELECT        tbl_EMPLOYEEDATA.*, 
                                            tbl_EMPLOYEEDATA_contact.EMPContact_no, 
                                            tbl_EMPLOYEEDATA_contact.EMPContact_address, 
                                            tbl_EMPLOYEEDATA_contact.EMPContact_number, 
                                            tbl_EMPLOYEEDATA_userdetail.EMPUser_name, 
                                            tbl_EMPLOYEEDATA_userdetail.EMPUser_password
                            FROM            tbl_EMPLOYEEDATA 
                            INNER JOIN      tbl_EMPLOYEEDATA_contact 
                            ON              tbl_EMPLOYEEDATA.EMP_ID = tbl_EMPLOYEEDATA_contact.EMP_ID 
                            INNER JOIN      tbl_EMPLOYEEDATA_userdetail 
                            ON              tbl_EMPLOYEEDATA.EMP_ID = tbl_EMPLOYEEDATA_userdetail.EMP_ID; ";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_EMPLOYEE.DataSource = table;

            dtg_emprecordLIST.ReadOnly = true;
            dtg_emprecordLIST.DataSource = bindingSource_EMPLOYEE;
        }
        // DATA GRIDVIEW ----------------------------------------------------------------------------------end

        // count total employee -------start
        private void totalEMP() {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             COUNT(EMP_ID) 
                             AS                 TOTAL
                             FROM               tbl_EMPLOYEEDATA";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_totalemployee.Text = myReader["TOTAL"].ToString();
                }
            }
            con.Close();
        }
        // count total employee -------end

        // count total user -------start
        private void totalUSER()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             COUNT(EMPUser_no) 
                             AS                 TOTAL
                             FROM               tbl_EMPLOYEEDATA_userdetail
                             WHERE              EMPUser_name != 'N/A'  AND EMPUser_name != NULL  ";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_totaluser.Text = myReader["TOTAL"].ToString();
                }
            }
            con.Close();
        }
        // count total user -------end


        // SEARCH ----------------------------------------------------------------------------------start
        private void txt_searchbox_TextChanged_1(object sender, EventArgs e)
        {
            con.ConnectionString = connectionString;
            con.Open();
            adapt = new SqlDataAdapter("SELECT * FROM tbl_EMPLOYEEDATA WHERE EMP_ID LIKE '  %" + txt_searchbox.Text + "' ", con);
            dt = new DataTable();
            adapt.Fill(dt);
            dtg_emprecordLIST.DataSource = dt;
            con.Close();
        }
    }
}
